using SteelX.Shared;
using System.Collections.Generic;
//using SteelX.Server.Shop;

namespace SteelX.Server.Packets.Shop
{
	/// <summary>
	/// Represents a single "chunk" or group of goods items
	/// This is pretty much all unknown
	/// </summary>
	public class SendGoodsData : ServerBasePacket
	{
		private readonly List<ShopEntry> _goods;
		
		public SendGoodsData(List<ShopEntry> goods)
		{
			_goods = goods;
		}

		public override Shared.PacketTypes PacketType
		{
			get
			{
				return Shared.PacketTypes.SEND_GOODS_DATA;
			}
		}

		/*public override string GetType()
		{
			return "SEND_GOODS_DATA";
		}

		public override byte GetId()
		{
			return 0x91;
		}*/

		protected override void WriteImpl()
		{           
			//TODO: Figure out this packet
			WriteInt(_goods.Count); // Number of goods in this packet

			foreach (var good in _goods)
			{
				WriteUInt(good.Id);
				WriteUInt(good.CreditPrice);
				WriteUInt(good.CoinPrice);
				WriteUInt(0); // Unknown
				
				WriteString(good.ItemNameCode);
				WriteString(good.ItemDescCode);
				
				WriteInt(good.Templates.Length);

				foreach (var template in good.Templates)
				{
					WriteInt(0); // Unknown
					WriteInt((int)good.ProductType);
					WriteUInt(template);
					WriteInt((int)good.ContractType);
					WriteUInt(good.ContractValue);
					WriteInt(0); // Unknown
				}
			}
		}
	}
}