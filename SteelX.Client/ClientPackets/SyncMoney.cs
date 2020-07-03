using SteelX.Shared;
using SteelX.Client.Packets.Inventory;

namespace SteelX.Client.Packets
{
	public class SyncMoney : ClientBasePacket
	{
		public override Shared.PacketTypes PacketType
		{
			get
			{
				return Shared.PacketTypes.SYNC_MONEY;
			}
		}

		/// <summary>
		/// Called when the user needs to sync their money
		/// appears to only be used in the shop
		/// </summary>
		/// <param name="data"></param>
		/// <param name="client"></param>
		public SyncMoney(byte[] data, GameSession client) : base(data, client)
		{
		}

		/*public override string GetType()
		{
			return "SYNC_MONEY";
		}*/

		protected override void RunImpl()
		{
			var client = GetClient();
			client.SendPacket(new SendMoneySynced(client.User));
		}
	}
}