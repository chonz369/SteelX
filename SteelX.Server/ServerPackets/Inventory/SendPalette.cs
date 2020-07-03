using SteelX.Shared;
using SteelX.Server;

namespace SteelX.Server.Packets.Inventory
{
	/// <summary>
	/// Sends the Palette?? for a unit
	/// Maybe equiped skills?
	/// </summary>
	public class SendPalette : ServerBasePacket
	{
		/// <summary>
		/// The unit that this packet is for
		/// </summary>
		private readonly Unit _unit;

		public SendPalette(Unit unit)
		{
			_unit = unit;
		}

		public override Shared.PacketTypes PacketType
		{
			get
			{
				return Shared.PacketTypes.INV_SEND_PALETTE;
			}
		}

		/*public override string GetType()
		{
			return "INV_SEND_PALETTE";
		}

		public override byte GetId()
		{
			return 0x23;
		}*/

		protected override void WriteImpl()
		{
			WriteUInt(_unit.Id);
			
			WriteInt(0); // Unknown
			WriteInt(0); // Unknown
			WriteInt(0); // Unknown
		}
	}
}