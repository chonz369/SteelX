using SteelX.Shared;

namespace SteelX.Server.Packets.Shop
{
	/// <summary>
	/// Sends the start indicator for the goods data download, along with the number of goods
	/// </summary>
	public class SendGoodsDataStart : ServerBasePacket
	{
		//TODO: This should be the actual number of goods
		//private readonly int _goodsSize = 9;
		private static int _goodsSize { get { return 9; } }

		public override Shared.PacketTypes PacketType
		{
			get
			{
				return Shared.PacketTypes.SEND_GOODS_DATA_START;
			}
		}

		/*public override string GetType()
		{
			return "SEND_GOODS_DATA_START";
		}

		public override byte GetId()
		{
			return 0x90;
		}*/

		protected override void WriteImpl()
		{
			WriteInt(_goodsSize);
		}
	}
}