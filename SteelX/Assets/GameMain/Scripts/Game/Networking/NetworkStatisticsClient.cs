
using UnityEngine;

public class NetworkStatisticsClient
{
    public FloatRollingAverage rtt { get { return m_RTT; } }

    private NetworkClient m_NetworkClient;

    public NetworkStatisticsClient(NetworkClient networkClient) {
        m_NetworkClient = networkClient;
    }

    public void Update() {
        m_RTT.Update(m_NetworkClient.rtt);

        if (NetworkConfig.netPrintStats.IntValue > 0) {
            if (Time.frameCount % NetworkConfig.netPrintStats.IntValue == 0) {
                PrintStats();
            }
        }
    }

    private void PrintStats() {
        var client = m_NetworkClient.clientConnection;
        if (client == null) return;

        Console.Write(string.Format("   {0,2} {1,-5} {2,-5} {3,-5} {4,-5} {5,-5} {6,-5} {7,-5} {8,-5} {9,-5}",
            "ID", "RTT", "ISEQ", "ITIM", "OSEQ", "OACK", "PLI", "PLO", "POOI", "PSI"));

        Console.Write(string.Format("   {0:00} {1,5} {2,5} {3,5} {4,5} {5,5} {6,5} {7,5} {8,5} {9,5}",
                client.ConnectionId, rtt.average, client.inSequence, client.inSequenceTime, client.outSequence, client.outSequenceAck,
                client.counters.packagesLostIn,
                client.counters.packagesLostOut,
                client.counters.packagesOutOfOrderIn,
                client.counters.packagesStaleIn
                ));
    }

    const int k_WindowSize = 120;
    public bool notifyHardCatchup;

    FloatRollingAverage m_RTT = new FloatRollingAverage(k_WindowSize);
}

