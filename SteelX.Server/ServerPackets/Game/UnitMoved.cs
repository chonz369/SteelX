using Data.Model;

namespace GameServer.ServerPackets.Game
{
    /// <summary>
    /// Sent to indicate that a unit has moved
    /// </summary>
    public class UnitMoved : ServerBasePacket
    {
        private readonly Unit _unit;

        public UnitMoved(Unit unit)
        {
            _unit = unit;
        }
        
        public override string GetType()
        {
            return "GAME_UNIT_MOVED";
        }

        public override byte GetId()
        {
            return 0x5e;
        }

        protected override void WriteImpl()
        {
            WriteUInt(_unit.Id);
            WriteUInt(_unit.User.Id);
            
            WriteByte(_unit.Movement);
            WriteByte(_unit.UnknownMovementFlag);
            WriteByte(_unit.Boosting);
            
            WriteFloat(_unit.WorldPosition.X);
            WriteFloat(_unit.WorldPosition.Y);
            WriteFloat(_unit.WorldPosition.Z);
            
            WriteFloat(0); // Unknown - vector?
            WriteFloat(0); // Unknown - vector?
            WriteFloat(0); // Unknown - vector?
            
            WriteShort(_unit.AimX);
            WriteShort(_unit.AimY);
        }
    }
}