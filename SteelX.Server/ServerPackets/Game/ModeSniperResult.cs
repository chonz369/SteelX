using Data.Model;

namespace GameServer.ServerPackets.Game
{
    /// <summary>
    /// Sent when a user enters sniper mode
    /// </summary>
    public class ModeSniperResult : ServerBasePacket
    {

        private readonly Unit _unit;

        public ModeSniperResult(Unit unit)
        {
            _unit = unit;
        }
        
        public override string GetType()
        {
            return "MODE_SNIPER";
        }

        public override byte GetId()
        {
            return 0x7d;
        }

        protected override void WriteImpl()
        {
            WriteInt(0); // Result?
            WriteUInt(_unit.Id);
            
            WriteShort(_unit.AimY);
            WriteShort(_unit.AimX);
            
            WriteFloat(_unit.WorldPosition.X); // Attacker - X
            WriteFloat(_unit.WorldPosition.Y); // Attacker - Y
            WriteFloat(_unit.WorldPosition.Z); // Attacker - Z
            
            WriteInt(0); // Unknown
        }
    }
}