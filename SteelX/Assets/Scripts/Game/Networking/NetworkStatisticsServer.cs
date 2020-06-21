using UnityEngine;

internal class NetworkStatisticsServer
{
    private NetworkServer m_NetworkServer;

    public NetworkStatisticsServer(NetworkServer networkServer) {
        m_NetworkServer = networkServer;
    }

    public void Update() {
        if (NetworkConfig.netPrintStats.IntValue > 0) {
            UpdateStats();
            if (Time.frameCount % NetworkConfig.netPrintStats.IntValue == 0) {
                PrintStats();
            }
        }
    }

    void UpdateStats() {
        foreach (var client in m_NetworkServer.GetConnections())
            client.Value.counters.UpdateAverages();
    }

    double lastStatsTime = 0;
    void PrintStats() {
        double timePassed = Game.frameTime - lastStatsTime;
        lastStatsTime = Game.frameTime;
        GameDebug.Log("Network stats");
        GameDebug.Log("=============");
        GameDebug.Log("Tick rate  : " + Game.serverTickRate.IntValue);
        //GameDebug.Log("Num netents: " + m_NetworkServer.NumEntities);
        Console.Write("--------------");
        Console.Write("Connections:");
        Console.Write("------------");
        Console.Write(string.Format("   {0,2} {1,-5} {2,-5} {3,-5} {4,-5} {5,-5} {6,-5} {7,-5} {8,-5} {9,-5}",
            "ID", "RTT", "ISEQ", "ITIM", "OSEQ", "OACK", "ppsI", "bpsI", "ppsO", "bpsO"));
        Console.Write("-------------------");
        int byteOutSum = 0;
        int byteOutCount = 0;
        foreach (var c in m_NetworkServer.GetConnections()) {
            var client = c.Value;
            Console.Write(string.Format("   {0:00} {1,5} {2,5} {3,5} {4,5} {5,5} {6:00.00}  {7,5}  {8:00.00} {9,5}",
                client.ConnectionId, client.rtt, client.inSequence, client.inSequenceTime, client.outSequence, client.outSequenceAck,
                (client.counters.avgPackagesIn.graph.average * Game.serverTickRate.FloatValue),
                (int)(client.counters.avgBytesIn.graph.average * Game.serverTickRate.FloatValue),
                (client.counters.avgPackagesOut.graph.average * Game.serverTickRate.FloatValue),
                (int)(client.counters.avgBytesOut.graph.average * Game.serverTickRate.FloatValue)
                ));
            byteOutSum += (int)(client.counters.avgBytesOut.graph.average * Game.serverTickRate.FloatValue);
            byteOutCount++;
        }
        if (byteOutCount > 0)
            Console.Write("Avg bytes out: " + (byteOutSum / byteOutCount));

        Console.Write("-------------------");
        var freq = NetworkConfig.netPrintStats.IntValue;
        //GameDebug.Log("Entity snapshots generated /frame : " + m_NetworkServer.statsGeneratedEntitySnapshots / freq);
        //GameDebug.Log("Generated worldsnapsize    /frame : " + m_NetworkServer.statsGeneratedSnapshotSize / freq);
        //GameDebug.Log("Entity snapshots total size/frame : " + m_NetworkServer.statsSnapshotData / freq);
        //GameDebug.Log("Updates sent               /frame : " + m_NetworkServer.statsSentUpdates / freq);
        //GameDebug.Log("Processed data outgoing      /sec : " + m_NetworkServer.statsProcessedOutgoing / timePassed);
        //GameDebug.Log("Sent data outgoing         /frame : " + m_NetworkServer.statsSentOutgoing / freq);
        //m_NetworkServer.statsGeneratedEntitySnapshots = 0;
        //m_NetworkServer.statsGeneratedSnapshotSize = 0;
        //m_NetworkServer.statsSnapshotData = 0;
        //m_NetworkServer.statsSentUpdates = 0;
        //m_NetworkServer.statsProcessedOutgoing = 0;
        //m_NetworkServer.statsSentOutgoing = 0;
        Console.Write("-------------------");
    }

    const int k_WindowSize = 120;
}




