using SteelX.Shared;
using SteelX.Server;

namespace SteelX.Server.Packets.Game
{
	/// <summary>
	/// Called when the game wants to spawn a unit
	/// </summary>
	public class SpawnUnit : ServerBasePacket
	{
		private readonly Mechanaught _unit;

		public SpawnUnit(Mechanaught unit)
		{
			_unit = unit;
		}

		public override Shared.PacketTypes PacketType
		{
			get
			{
				return Shared.PacketTypes.GAME_SPAWN_UNIT;
			}
		}

		/*public override string GetType()
		{
			return "GAME_SPAWN_UNIT";
		}

		public override byte GetId()
		{
			return 0x4d;
		}*/

		protected override void WriteImpl()
		{
			//TODO: Move this to helper function?
			//TODO: Spawn maps?
			
			WriteUInt(_unit.Id);
			WriteInt(0); // Unknown - weapon set?
			
			//TODO: WriteVector3?
			WriteFloat(_unit.WorldPosition.X);
			WriteFloat(_unit.WorldPosition.Y);
			WriteFloat(_unit.WorldPosition.Z);
			
			//TODO: WriteVector2?
			WriteShort(_unit.AimY);
			WriteShort(_unit.AimX);
		}
	}
}