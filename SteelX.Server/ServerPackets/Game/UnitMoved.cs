using System;
//using System.Numerics;
using SteelX.Shared;
using SteelX.Server;
using SteelX.Shared.Utility;

namespace SteelX.Server.Packets.Game
{
	/// <summary>
	/// Sent to indicate that a unit has moved
	/// </summary>
	public class UnitMoved : ServerBasePacket
	{
		private readonly Mechanaught _unit;
		private readonly int _serverTime;
		private readonly Vector _vel;

		public UnitMoved(Mechanaught unit, int serverTime, Vector vel)
		{
			_unit = unit;
			_serverTime = serverTime;
			_vel = vel;
		}

		public override Shared.PacketTypes PacketType
		{
			get
			{
				return Shared.PacketTypes.GAME_UNIT_MOVED;
			}
		}

		/*public override string GetType()
		{
			return "GAME_UNIT_MOVED";
		}

		public override byte GetId()
		{
			return 0x63;
		}*/

		protected override void WriteImpl()
		{
			WriteInt(_serverTime); // Unknown - Tick?
			WriteUInt(_unit.Id); 
			
			WriteByte(_unit.Movement);
			WriteByte(_unit.UnknownMovementFlag);
			WriteByte(_unit.Boosting? (byte)1 : (byte)0);
			
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