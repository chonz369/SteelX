using Data.Model;

namespace GameServer.ServerPackets.Bridge.Stats
{   
    /// <summary>
    /// A packet containing stats for survival (deathmatch)
    /// </summary>
    public class SurvivalInfo : ServerBasePacket
    {
        private readonly UserStats _stats;
        
        public SurvivalInfo(UserStats stats)
        {
            _stats = stats;
        }
        
        public override string GetType()
        {
            return $"BRIDGE_SEND_SURVIVAL_INFO";
        }

        public override byte GetId()
        {
            return 0x99;
        }

        protected override void WriteImpl()
        {
            // Totally unknown
            // 12 Ints
            WriteInt(0);
            WriteInt(0);
            WriteInt(69); // Kills
            WriteInt(100); // Assist
            WriteInt(2); // Deaths
            WriteInt(11); // 1st place wins
            WriteInt(0); 
            WriteInt(0);
            WriteInt(3); // Desertions
            WriteInt(500); // Points
            WriteInt(1000); // High Score
            WriteInt(3600); // Time
            WriteInt(0);
        }
    }
}