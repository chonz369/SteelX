using SteelX.Shared;
using SteelX.Server;

namespace SteelX.Server.Packets.Inventory
{
	/// <summary>
	/// Sends all the parts (and other things?) in the users inventory
	/// </summary>
	public class SendParts : ServerInventoryBasePacket
	{
		public SendParts(Player user) : base(user)
		{
		}

		public override Shared.PacketTypes PacketType
		{
			get
			{
				return Shared.PacketTypes.INV_SEND_PARTS;
			}
		}

		/*public override string GetType()
		{
			return "INV_SEND_PARTS";
		}

		public override byte GetId()
		{
			return 0x20;
		}*/

		protected override void WriteImpl()
		{
			WriteInt(Inventory.Parts.Count);

			foreach (var part in Inventory.Parts)
			{
				this.WritePartInfo(part);
			}
		}
	}
}