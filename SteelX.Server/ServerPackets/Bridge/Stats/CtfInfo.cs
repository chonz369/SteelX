using Data.Model;

namespace GameServer.ServerPackets.Bridge.Stats
{   
    /// <summary>
    /// A packet containing stats for team battle (territory control)
    /// </summary>
    public class CtfInfo : ServerBasePacket
    {
        private readonly UserStats _stats;
        
        public CtfInfo(UserStats stats)
        {
            _stats = stats;
        }
        
        public override string GetType()
        {
            return $"BRIDGE_SEND_CTF_INFO";
        }

        public override byte GetId()
        {
            return 0x9c;
        }

        protected override void WriteImpl()
        {
            // Totally unknown
            // 12 Ints
            WriteInt(0); 
            WriteInt(1);
            WriteInt(2); // Kills 
            WriteInt(3); // Assists
            WriteInt(4); // Deaths
            WriteInt(5); // Wins
            WriteInt(6); // Losses
            WriteInt(7); // Draws
            WriteInt(8); // Desertions
            WriteInt(9); // Flag captures
            WriteInt(10); // 
            WriteInt(11); // 
            WriteInt(12); // 
            WriteInt(3600); // 
            WriteInt(0); // 
        }
    }
}