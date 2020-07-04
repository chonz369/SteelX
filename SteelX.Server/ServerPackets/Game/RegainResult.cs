using SteelX.Shared;
using SteelX.Server;

namespace SteelX.Server.Packets.Game
{
	/// <summary>
	/// Sent when the user sucessfully or unsucessfully respawns i think
	/// </summary>
	public class RegainResult : ServerBasePacket
	{
		private readonly Mechanaught _unit;

		public RegainResult(Mechanaught unit)
		{
			_unit = unit;
		}

		public override Shared.PacketTypes PacketType
		{
			get
			{
				return Shared.PacketTypes.GAME_REGAIN_RESULT;
			}
		}

		/*public override string GetType()
		{
			return "GAME_REGAIN_RESULT";
		}

		public override byte GetId()
		{
			return 0x57;
		}*/

		protected override void WriteImpl()
		{
			WriteInt(0); // Result code?
			WriteUInt(_unit.Id); // Unit id?
		}
	}
}