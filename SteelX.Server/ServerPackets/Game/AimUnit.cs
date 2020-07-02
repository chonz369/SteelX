using Data.Model;

namespace GameServer.ServerPackets.Game
{
    /// <summary>
    /// I think this is the aim unit packet, but it has a lot of data so i could be wrong
    /// </summary>
    public class AimUnit : ServerBasePacket
    {
        private readonly Unit _attacker;
        private readonly uint _victim;
        
        public AimUnit(Unit attacker, uint victim)
        {
            _attacker = attacker;
            _victim = victim;
        }
        
        public override string GetType()
        {
            return "AIM_UNIT";
        }

        public override byte GetId()
        {
            return 0x65;
        }

        protected override void WriteImpl()
        {
            WriteInt(0); // Unknown
            WriteUInt(_attacker.Id); // UnitID Attacker
            WriteUInt(_victim); // UnitId Victim - zero means no lock?
            WriteInt(0); // Unknown
            WriteInt(0); // Unknown
            
            WriteShort(_attacker.AimY); // Attacker - AimX
            WriteShort(_attacker.AimX); // Attacker - AimY
            
            WriteFloat(_attacker.WorldPosition.X); // Attacker - X
            WriteFloat(_attacker.WorldPosition.Y); // Attacker - Y
            WriteFloat(_attacker.WorldPosition.Z); // Attacker - Z
        }
    }
}