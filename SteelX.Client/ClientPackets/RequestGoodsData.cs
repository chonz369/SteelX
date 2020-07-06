using SteelX.Shared;
//using GameServer.Config;
//using GameServer.ServerPackets.Shop;

namespace SteelX.Client.Packets
{
	/// <summary>
	/// Called when the client needs to download shop data
	/// </summary>
	/// Totally unknown
	public class RequestGoodsData : ClientBasePacket
	{
		public override Shared.PacketTypes PacketType
		{
			get
			{
				return Shared.PacketTypes.REQ_SHOP_DATA;
			}
		}

		/// <summary>
		/// </summary>
		/// <param name="data"></param>
		/// <param name="client"></param>
		public RequestGoodsData(byte[] data, GameSession client) : base(data, client)
		{
		}

		/*public override string GetType()
		{
			return "REQ_GOODS_DATA";
		}*/

		protected override void RunImpl()
		{
			//TODO: This should run in another thread?
			var client = GetClient();
			
			//Sends a request to server saying client is accessing shop
			//Server will respond with all shop items to display
			//Shop data is unlikely to change mid gameplay
			//so data stream can be cached if accessed once already
			//every user sees the same items, and every user gets the same price
			/*client.SendPacket(new SendGoodsDataStart());
			
			client.SendPacket(new SendGoodsData(ShopDataReader.Set));
			
			client.SendPacket(new SendGoodsData(ShopDataReader.Head));
			client.SendPacket(new SendGoodsData(ShopDataReader.Chest));
			client.SendPacket(new SendGoodsData(ShopDataReader.Arm));
			client.SendPacket(new SendGoodsData(ShopDataReader.Leg));
			client.SendPacket(new SendGoodsData(ShopDataReader.Booster));
			
			client.SendPacket(new SendGoodsData(ShopDataReader.Weapon));
			
			client.SendPacket(new SendGoodsData(ShopDataReader.Code));
			client.SendPacket(new SendGoodsData(ShopDataReader.Etc));
			
			client.SendPacket(new SendGoodsDataEnd());*/
		}
	}
}