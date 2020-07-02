using System;
using System.Numerics;
using Data.Model;

namespace GameServer.ServerPackets.Game
{
    /// <summary>
    /// Sent to indicate that a unit has moved
    /// </summary>
    public class UnitMoved : ServerBasePacket
    {
        private readonly Unit _unit;
        private readonly int _serverTime;
        private readonly Vector3 _vel;

        public UnitMoved(Unit unit, int serverTime, Vector3 vel)
        {
            _unit = unit;
            _serverTime = serverTime;
            _vel = vel;
        }
        
        public override string GetType()
        {
            return "GAME_UNIT_MOVED";
        }

        public override byte GetId()
        {
            return 0x63;
        }

        protected override void WriteImpl()
        {
            WriteInt(_serverTime); // Unknown - Tick?
            WriteUInt(_unit.Id); 
            
            WriteByte(_unit.Movement);
            WriteByte(_unit.UnknownMovementFlag);
            WriteByte(_unit.Boosting);
            
            WriteFloat(_unit.WorldPosition.X);
            WriteFloat(_unit.WorldPosition.Y);
            WriteFloat(_unit.WorldPosition.Z);
            
            WriteFloat(_vel.X); // Unknown - vector?
            WriteFloat(_vel.Y); // Unknown - vector?
            WriteFloat(_vel.Z); // Unknown - vector?
            
            WriteShort(_unit.AimY);
            WriteShort(_unit.AimX);
        }
    }
}