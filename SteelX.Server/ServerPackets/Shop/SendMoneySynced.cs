using SteelX.Shared;
using SteelX.Server;
//using SteelX.Server.Items;

namespace SteelX.Server.Packets.Inventory
{
	/// <summary>
	/// Sends the users money and coins
	/// </summary>
	public class SendMoneySynced : ServerInventoryBasePacket
	{
		public SendMoneySynced(Player user) : base(user) { }

		public override Shared.PacketTypes PacketType
		{
			get
			{
				return Shared.PacketTypes.MONEY_SYNCED;
			}
		}

		/*public override string GetType()
		{
			return "MONEY_SYNCED";
		}

		public override byte GetId()
		{
			return 0x10;
		}*/

		protected override void WriteImpl()
		{
			//WriteUInt(Inventory.User.Credits);
			//WriteUInt(Inventory.User.Coins);
		}
	}
}