using SteelX.Shared;
using SteelX.Server;

namespace SteelX.Server.Packets.Inventory
{
	/// <summary>
	/// Sends special items
	/// Need to investigate further
	/// </summary>
	public class SendSpecialItems : ServerInventoryBasePacket
	{
		public SendSpecialItems(Player user) : base(user)
		{
		}

		public override Shared.PacketTypes PacketType
		{
			get
			{
				return Shared.PacketTypes.INV_SPECIAL_ITEM_LIST;
			}
		}

		/*public override string GetType()
		{
			return "INV_SPECIAL_ITEM_LIST";
		}

		public override byte GetId()
		{
			return 0x25;
		}*/

		protected override void WriteImpl()
		{
			WriteInt(0); // Unknown - Special item count?
		}
	}
}