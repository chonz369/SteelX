using Data.Model;

namespace GameServer.ServerPackets.Bridge.Stats
{   
    /// <summary>
    /// A packet containing stats for survival (deathmatch)
    /// </summary>
    public class ClanBattleInfo : ServerBasePacket
    {
        private readonly UserStats _stats;
        
        public ClanBattleInfo(UserStats stats)
        {
            _stats = stats;
        }
        
        public override string GetType()
        {
            return $"BRIDGE_SEND_CLANBATTLE_INFO";
        }

        public override byte GetId()
        {
            return 0x9d;
        }

        protected override void WriteImpl()
        {
            // Totally unknown
            // 12 Ints
            WriteInt(0);
            WriteInt(1);
            WriteInt(2); 
            WriteInt(3); 
            WriteInt(4); 
            WriteInt(5); 
            WriteInt(6); 
            WriteInt(7);
            WriteInt(8); 
            WriteInt(9); 
            WriteInt(10); 
            WriteInt(11);
            WriteInt(12);
        }
    }
}