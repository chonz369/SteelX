using SteelX.Shared;
using SteelX.Server;

namespace SteelX.Server.Packets.Inventory
{
	/// <summary>
	/// Send the number of unit slots this user has
	/// </summary>
	public class SendUnitSlots : ServerInventoryBasePacket
	{
		public SendUnitSlots(Player user) : base(user)
		{
		}

		public override Shared.PacketTypes PacketType
		{
			get
			{
				return Shared.PacketTypes.INV_SEND_UNIT_SLOTS;
			}
		}

		/*public override string GetType()
		{
			return "INV_SEND_UNIT_SLOTS";
		}

		public override byte GetId()
		{
			return 0x21;
		}*/

		protected override void WriteImpl()
		{
			WriteUInt(Inventory.UnitSlots);
		}
	}
}