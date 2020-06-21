using NetworkCompression;
using System;
using System.Collections.Generic;

public class PackageInfo
{
    public long SentTime;
    public NetworkMessage Content;

    public List<NetworkEvent> events = new List<NetworkEvent>(10);

    public virtual void Reset() {
        SentTime = 0;
        Content = 0;
        foreach (var eventInfo in events)
            eventInfo.Release();
        events.Clear();
    }
}

public class NetworkConnectionCounters
{
    public int bytesIn;                         // The number of user bytes received on this connection
    public int bytesOut;                        // The number of user bytes sent on this connection

    public int packagesIn;                      // The number of packages received on this connection
    public int packagesOut;                     // The number of packages sent on this connection

    public int packagesStaleIn;                 // The number of state packages we received
    public int packagesDuplicateIn;             // The number of duplicate packages we received
    public int packagesOutOfOrderIn;            // The number of packages we received out of order

    public int packagesLostIn;                  // The number of incoming packages that was lost (i.e. holes in the package sequence)
    public int packagesLostOut;                 // The number of outgoing packages that wasn't acked (either due to choke or network)

    public int eventsIn;                        // The total number of events received
    public int eventsOut;                       // The total number of events sent
    public int eventsLostOut;                   // The number of events that was lost

    public int reliableEventsOut;               // The number of reliable events sent
    public int reliableEventResendOut;          // The number of reliable events we had to resend

    public Aggregator avgBytesIn = new Aggregator();
    public Aggregator avgBytesOut = new Aggregator();
    public Aggregator avgPackagesIn = new Aggregator();
    public Aggregator avgPackagesOut = new Aggregator();
    public Aggregator avgPackageSize = new Aggregator();

    public void UpdateAverages() {
        avgBytesIn.Update(bytesIn);
        avgBytesOut.Update(bytesOut);
        avgPackagesIn.Update(packagesIn);
        avgPackagesOut.Update(packagesOut);
    }
}

public class NetworkConnection<TPackageInfo, TCounters> where TPackageInfo : PackageInfo, new()
    where TCounters : NetworkConnectionCounters, new()
{
    public int ConnectionId;
    public INetworkTransport Transport;
    public TCounters counters = new TCounters();

    public int rtt;                                 // Round trip time (ping + time lost due to read/send frequencies)

    public int inSequence;                          // The highest sequence of packages we have received
    public ushort inSequenceAckMask;                // The mask describing which of the last packages we have received relative to inSequence
    public long inSequenceTime;                     // The time the last package was received

    public int outSequence = 1;                     // The sequence of the next outgoing package
    public int outSequenceAck;                      // The highest sequence of packages that have been acked
    public ushort outSequenceAckMask;               // The mask describing which of the last packaged have been acked related to outSequence

    public NetworkConnection(int connectionId, INetworkTransport transport) {
        ConnectionId = connectionId;
        Transport = transport;
    }

    public int ProcessPackageHeader(byte[] packageData, out NetworkMessage content, out int headerSize) {
        counters.packagesIn++;

        var input = new BitInputStream(packageData);
        
        headerSize = 0;
        int headerStartInBits = input.GetBitPosition();

        content = (NetworkMessage)input.ReadBits(8);

        var inSequenceNew = Sequence.FromUInt16((ushort)input.ReadBits(16), inSequence);
        var outSequenceAckNew = Sequence.FromUInt16((ushort)input.ReadBits(16), outSequenceAck);
        var outSequenceAckMaskNew = (ushort)input.ReadBits(16);

        if(inSequenceNew > inSequence) {
            // If we have a hole in the package sequence that will fall off the ack mask that 
            // means the package (inSequenceNew-15 and before) will be considered lost (either it will never come or we will 
            // reject it as being stale if we get it at a later point in time)
            var distance = inSequenceNew - inSequence;
            for (var i = 0; i < Math.Min(distance, 15); ++i)    // TODO : Fix this contant
            {
                if ((inSequenceAckMask & 1 << (15 - i)) == 0)
                    counters.packagesLostIn++;
            }

            // If there is a really big hole then those packages are considered lost as well
            // Update the incoming ack mask.
            if (distance > 15) {
                counters.packagesLostIn += distance - 15;
                inSequenceAckMask = 1; // all is lost except current package
            } else {
                inSequenceAckMask <<= distance;
                inSequenceAckMask |= 1;
            }

            inSequence = inSequenceNew;
            inSequenceTime = NetworkUtils.stopwatch.ElapsedMilliseconds;
        } else if (inSequenceNew < inSequence) {
            // Package is out of order

            // Check if the package is stale
            var distance = inSequence - inSequenceNew;
            if (distance > 15) // TODO : Fix this constant
            {
                counters.packagesStaleIn++;
                return 0;
            }

            // Check if the package is a duplicate
            var ackBit = 1 << distance;
            if ((ackBit & inSequenceAckMask) != 0) {
                // Duplicate package
                counters.packagesDuplicateIn++;
                return 0;
            }

            // Accept the package out of order
            //counters.packagesOutOfOrderIn++;
            inSequenceAckMask |= (ushort)ackBit;
        } else {
            // Duplicate package
            counters.packagesDuplicateIn++;
            return 0;
        }

        if (inSequenceNew % 3 == 0) {
            var timeOnServer = (ushort)input.ReadBits(8);
            TPackageInfo info;
            if (outstandingPackages.TryGetValue(outSequenceAckNew, out info)) {
                var now = NetworkUtils.stopwatch.ElapsedMilliseconds;
                rtt = (int)(now - info.SentTime - timeOnServer);
            }
        }

        // If the ack sequence is not higher we have nothing new to do
        if (outSequenceAckNew <= outSequenceAck) {
            headerSize = input.Align();
            return inSequenceNew;
        }

        // Find the sequence numbers that we have to consider lost
        var seqsBeforeThisAlreadyNotifedAsLost = outSequenceAck - 15;
        var seqsBeforeThisAreLost = outSequenceAckNew - 15;

        for (int sequence = seqsBeforeThisAlreadyNotifedAsLost; sequence <= seqsBeforeThisAreLost; ++sequence) {
            // Handle conditions before first 15 packets
            if (sequence < 0)
                continue;

            // If seqence covered by old ack mask, we may already have received it (and notified)
            int bitnum = outSequenceAck - sequence;
            var ackBit = bitnum >= 0 ? 1 << bitnum : 0;
            var notNotified = (ackBit & outSequenceAckMask) == 0;

            if (outstandingPackages.Exists(sequence) && notNotified) {
                var info = outstandingPackages[sequence];
                NotifyDelivered(sequence, info, false);

                counters.packagesLostOut++;

                info.Reset();
                outstandingPackages.Remove(sequence);
            }
        }

        outSequenceAck = outSequenceAckNew;
        outSequenceAckMask = outSequenceAckMaskNew;

        // Ack packages if they haven't been acked already
        for (var sequence = Math.Max(outSequenceAck - 15, 0); sequence <= outSequenceAck; ++sequence) {
            var ackBit = 1 << outSequenceAck - sequence;
            if (outstandingPackages.Exists(sequence) && (ackBit & outSequenceAckMask) != 0) {
                var info = outstandingPackages[sequence];
                NotifyDelivered(sequence, info, true);

                info.Reset();
                outstandingPackages.Remove(sequence);
            }
        }

        headerSize = input.Align();
        return inSequenceNew;
    }

    public void QueueEvent(NetworkEvent info) {
        eventsOut.Add(info);
        info.AddRef();
    }

    public void WriteEvents(TPackageInfo info, ref RawOutputStream output) {
        if (eventsOut.Count == 0)
            return;

        foreach (var eventInfo in eventsOut) {
            counters.eventsOut++;
            if (eventInfo.reliable)
                counters.reliableEventsOut++;
        }

        AddMessageContentFlag(NetworkMessage.Events);

        GameDebug.Assert(info.events.Count == 0);
        NetworkEvent.WriteEvents(eventsOut, ackedEventTypes, ref output);
        info.events.AddRange(eventsOut);
        eventsOut.Clear();
    }

    public void ReadEvents(ref RawInputStream input, INetworkCallbacks networkConsumer){
        var numEvents = NetworkEvent.ReadEvents(eventTypesIn, ConnectionId, ref input, networkConsumer);
        counters.eventsIn += numEvents;
    }

    public void BeginSendPackage(ref BitOutputStream output, out TPackageInfo info) {
        output.WriteBits(0, 8);                                 // Package content flags (will set later as we add messages)
        output.WriteBits(Sequence.ToUInt16(outSequence), 16);
        output.WriteBits(Sequence.ToUInt16(inSequence), 16);
        output.WriteBits(inSequenceAckMask, 16);

        // Send rtt info every 3th package. We calculate the RTT as the time from sending the package
        // and receiving the ack for the package minus the time the package spent on the server
        if (outSequence % 3 == 0) {
            var now = NetworkUtils.stopwatch.ElapsedMilliseconds;
            var timeOnServer = (byte)Math.Min(now - inSequenceTime, 255);
            output.WriteBits(timeOnServer, 8);
        }

        info = outstandingPackages.Acquire(outSequence);
    }

    protected void AddMessageContentFlag(NetworkMessage message) {
        m_PackageBuffer[0] |= (byte)message;
    }

    protected int CompleteSendPackage(TPackageInfo info, ref BitOutputStream output) {
        info.SentTime = NetworkUtils.stopwatch.ElapsedMilliseconds;
        info.Content = (NetworkMessage)m_PackageBuffer[0];
        int packageSize = output.Flush();

        byte[] data = new byte[packageSize];
        NetworkUtils.MemCopy(m_PackageBuffer, 0, data, 0, packageSize);

        counters.bytesOut += data.Length;
        Transport.SendData(ConnectionId, TransportEvent.Type.Data, data);

        counters.packagesOut++;
        ++outSequence;

        return packageSize;
    }

    protected virtual void NotifyDelivered(int sequence, TPackageInfo info, bool madeIt) {
        if (madeIt) {
            // Release received reliable events
            foreach (var eventInfo in info.events) {
                if (!ackedEventTypes.Contains(eventInfo.type))
                    ackedEventTypes.Add(eventInfo.type);
                eventInfo.Release();
            }
        } else {
            foreach (var eventInfo in info.events) {
                counters.eventsLostOut++;
                if (eventInfo.reliable) {
                    // Re-add dropped reliable events to outgoing events
                    counters.reliableEventResendOut++;
                    GameDebug.Log("Resending lost reliable event: " + ((GameNetworkEvents.EventType)eventInfo.type.typeId) + ":" + eventInfo.sequence);
                    eventsOut.Add(eventInfo);
                } else
                    eventInfo.Release();
            }
        }
        info.events.Clear();
    }

    public byte[] m_PackageBuffer = new byte[NetworkConfig.MaxPackageSize];

    List<NetworkEventType> ackedEventTypes = new List<NetworkEventType>();
    Dictionary<ushort, NetworkEventType> eventTypesIn = new Dictionary<ushort, NetworkEventType>();
    public SequenceBuffer<TPackageInfo> outstandingPackages = new SequenceBuffer<TPackageInfo>(64, () => new TPackageInfo());
    private List<NetworkEvent> eventsOut = new List<NetworkEvent>();
}

