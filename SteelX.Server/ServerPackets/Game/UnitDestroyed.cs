using Data.Model;

namespace GameServer.ServerPackets.Game
{
    /// <summary>
    /// Sent when someone DIES
    /// </summary>
    public class UnitDestroyed : ServerBasePacket
    {
        private readonly Unit _killer;
        private readonly Unit _victim;

        public UnitDestroyed(Unit victim, Unit killer)
        {
            _killer = killer;
            _victim = victim;
        }
        
        public override string GetType()
        {
            return "UNIT_DESTROYED";
        }

        public override byte GetId()
        {
            return 0x62;
        }

        protected override void WriteImpl()
        {
            var killerId = _killer?.Id ?? 0;
            
            WriteInt(0); // Unknown
            WriteUInt(killerId); // Killer
            WriteUInt(_victim.Id); // VictimId
            WriteInt(1); // Unknown
        }
    }
}