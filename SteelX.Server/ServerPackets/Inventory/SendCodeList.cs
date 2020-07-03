using SteelX.Shared;
using SteelX.Server;

namespace SteelX.Server.Packets.Inventory
{
	/// <summary>
	/// This looks like a packet for skills in the users inventory
	/// </summary>
	public class SendCodeList : ServerInventoryBasePacket
	{
		public SendCodeList(Player user) : base(user)
		{
		}

		public override Shared.PacketTypes PacketType
		{
			get
			{
				return Shared.PacketTypes.INV_CODE_LIST;
			}
		}

		/*public override string GetType()
		{
			return "INV_CODE_LIST";
		}

		public override byte GetId()
		{
			return 0x1f;
		*/

		protected override void WriteImpl()
		{
			// All unknown
			WriteInt(0); // Unknown
			WriteInt(0); // Unknown
		}
	}
}