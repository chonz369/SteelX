using System.Collections.Generic;
using Data.Model.Shop;

namespace GameServer.ServerPackets.Shop
{
    /// <summary>
    /// Represents a single "chunk" or group of goods items
    /// This is pretty much all unknown
    /// </summary>
    public class SendGoodsData : ServerBasePacket
    {
        private readonly List<Good> _goods;
        
        public SendGoodsData(List<Good> goods)
        {
            _goods = goods;
        }
        
        public override string GetType()
        {
            return "SEND_GOODS_DATA";
        }

        public override byte GetId()
        {
            return 0x8b;
        }

        protected override void WriteImpl()
        {
            // TODO: Figure out this packet
            WriteInt(_goods.Count); // Number of goods in this packet

            foreach (var good in _goods)
            {
                WriteUInt(good.Id);
                WriteUInt(good.CreditPrice);
                WriteUInt(good.CoinPrice);
                
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

//            for (var i = 1; i <= 52; i++)
//            {
//
//                // All below is unknown
//                WriteInt(1000 + i); // Possible ID
//                WriteInt(1000); // Credit Price
//                WriteInt(0); // NCoin price
//
//                WriteString(" "); // Item name code (en_us.jof)
//                WriteString(" "); // Desc code (en_us.jof)
//
//                // Number of unknown structure
//                WriteInt(1);
//
//                // Unknown structure(s)
//                WriteInt(0); // Unknown
//                WriteInt(7); // Seems to be item type or category? 0 = parts, 1 = code/skill, 2 = etc, 3 = Unit set
//                WriteInt(7770000 + i); // Template Id
//                WriteInt(1); // 0 - fixed qty, 1 - durability, 2 - time period
//                WriteInt(10000); // Value for previous - qty, durability, or time in days
//                WriteInt(0); // Unknown
//            }


//            // All below is unknown
//            WriteInt(1002); // Unknown
//            WriteInt(0); // Credit Price
//            WriteInt(0); // NCoin price
//            
//            WriteString("UnitSlotA_1"); // Item name code (en_us.jof)
//            WriteString("Unit_UnitSlotA_1"); // Desc code (en_us.jof)
//            
//            // Number of unknown structure
//            WriteInt(4);
//            
//            // Unknown structure(s)
//            WriteInt(1000); // Unknown
//            WriteInt(0); // Seems to be item type or category? 0 = parts, 1 = code/skill, 2 = etc, 3 = Unit set
//            WriteInt(1110002); // Template Id
//            WriteInt(1); // 0 - fixed qty, 1 - durability, 2 - time period
//            WriteInt(10000); // Value for previous - qty, durability, or time in days
//            WriteInt(0); // Unknown
//            
//            WriteInt(1001); // Unknown
//            WriteInt(0); // Seems to be item type or category? 0 = parts, 1 = code/skill, 2 = etc, 3 = Unit set
//            WriteInt(2220002); // Template Id
//            WriteInt(1); // 0 - fixed qty, 1 - durability, 2 - time period
//            WriteInt(10000); // Value for previous - qty, durability, or time in days
//            WriteInt(0); // Unknown
//            
//            WriteInt(0); // Unknown
//            WriteInt(0); // Seems to be item type or category? 0 = parts, 1 = code/skill, 2 = etc, 3 = Unit set
//            WriteInt(3330002); // Template Id
//            WriteInt(1); // 0 - fixed qty, 1 - durability, 2 - time period
//            WriteInt(10000); // Value for previous - qty, durability, or time in days
//            WriteInt(0); // Unknown
//            
//            WriteInt(0); // Unknown
//            WriteInt(0); // Seems to be item type or category? 0 = parts, 1 = code/skill, 2 = etc, 3 = Unit set
//            WriteInt(4440002); // Template Id
//            WriteInt(1); // 0 - fixed qty, 1 - durability, 2 - time period
//            WriteInt(10000); // Value for previous - qty, durability, or time in days
//            WriteInt(0); // Unknown
        }
    }
}