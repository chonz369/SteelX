using SteelX.Shared;

namespace SteelX.Server.Packets.Shop
{
	public class SendGoodsDataEnd : ServerBasePacket
	{
		public override Shared.PacketTypes PacketType
		{
			get
			{
				return Shared.PacketTypes.SEND_GOODS_DATA_END;
			}
		}

		/*public override string GetType()
		{
			return "SEND_GOODS_DATA_END";
		}

		public override byte GetId()
		{
			return 0x92;
		}*/

		// Empty packet
		protected override void WriteImpl() {}
	}
}