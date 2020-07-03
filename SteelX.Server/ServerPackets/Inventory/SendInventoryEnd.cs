using SteelX.Shared;
using SteelX.Server;

namespace SteelX.Server.Packets.Inventory
{
	/// <summary>
	/// Sends when all other inventory packets have been transmitted
	/// </summary>
	public class SendInventoryEnd : ServerInventoryBasePacket
	{
		public SendInventoryEnd(Player user) : base(user)
		{
		}

		public override Shared.PacketTypes PacketType
		{
			get
			{
				return Shared.PacketTypes.INV_END;
			}
		}

		/*public override string GetType()
		{
			return "INV_END";
		}

		public override byte GetId()
		{
			return 0x26;
		}*/

		protected override void WriteImpl()
		{
			WriteInt(Inventory.InventoryUsed);
			WriteUInt(Inventory.InventorySize);
			
			WriteInt(0); // Unknown
			WriteInt(0); // Unknown
			WriteInt(0); // Unknown
			WriteInt(0); // Unknown
			
			// FArray - Unknown
			WriteInt(0); // Unknown - Size?
			
			// FArray - Unknown
			WriteInt(0); // Unknown - Size?
		}
	}
}