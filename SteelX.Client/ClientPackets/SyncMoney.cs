using SteelX.Shared;
using SteelX.Client.Packets.Inventory;

namespace SteelX.Client.Packets
{
	/// <summary>
	/// Called when the user needs to sync their money
	/// </summary>
	/// appears to only be used in the shop
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
			//Sends a request to server to refresh/update the credits end user has
			//Server responds back with int value of credits and premium currency
			//Update enduser with information in heads-up display
			//Data is read-only and cannot be manipulated (immutable)
			//Validation checks are done on server, not client
			//var client = GetClient();
			//client.SendPacket(new SendMoneySynced(client.User));
		}
	}
}