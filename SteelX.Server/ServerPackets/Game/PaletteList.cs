using SteelX.Shared;
using SteelX.Server;

namespace SteelX.Server.Packets.Game
{
	/// <summary>
	/// Seems to be sent with a list of "palette"
	/// </summary>
	/// Maybe skills?
	public class PaletteList : ServerBasePacket
	{
		/// <summary>
		/// The unit whos data to send
		/// </summary>
		private readonly Mechanaught _unit;

		public PaletteList(Mechanaught unit)
		{
			_unit = unit;
		}

		public override Shared.PacketTypes PacketType
		{
			get
			{
				return Shared.PacketTypes.GAME_PALETTE_LIST;
			}
		}

		/*public override string GetType()
		{
			return "GAME_PALETTE_LIST";
		}

		public override byte GetId()
		{
			return 0x4b;
		}*/

		protected override void WriteImpl()
		{
			WriteUInt(_unit.Id);
			
			WriteInt(0); // Unknown
			WriteInt(0); // Unknown
		}
	}
}