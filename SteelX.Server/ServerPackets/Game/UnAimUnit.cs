using Data.Model;

namespace GameServer.ServerPackets.Game
{
    /// <summary>
    /// I think this is the aim unit packet, but it has a lot of data so i could be wrong
    /// </summary>
    public class UnAimUnit : ServerBasePacket
    {
        private readonly Unit _attacker;
        private readonly uint _victim;
        
        public UnAimUnit(Unit attacker, uint victim)
        {
            _attacker = attacker;
            _victim = victim;
        }
        
        public override string GetType()
        {
            return "UN_AIM_UNIT";
        }

        public override byte GetId()
        {
            return 0x66;
        }

        protected override void WriteImpl()
        {
            WriteInt(0); // Unknown
            WriteUInt(_attacker.Id); // UnitID Attacker
            WriteUInt(_victim); // UnitId Victim - zero means no lock?
            WriteInt(0); // Unknown
            WriteInt(0); // Unknown
        }
    }
}