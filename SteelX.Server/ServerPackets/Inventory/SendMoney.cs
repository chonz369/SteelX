using SteelX.Shared;
using SteelX.Server;
//using SteelX.Server.Items;

namespace SteelX.Server.Packets.Inventory
{
	/// <summary>
	/// Sends the users money and coins
	/// </summary>
	public class SendMoney : ServerInventoryBasePacket
	{
		public SendMoney(Player user) : base(user) { }

		public override Shared.PacketTypes PacketType
		{
			get
			{
				return Shared.PacketTypes.INV_SEND_MONEY;
			}
		}

		/*public override string GetType()
		{
			// Not sure exact packet name in client
			return "INV_SEND_MONEY";
		}

		public override byte GetId()
		{
			return 0x1d;
		}*/

		protected override void WriteImpl()
		{
			WriteUInt(Inventory.User.Coins);
			WriteUInt(Inventory.User.Credits);
		}
	}
}