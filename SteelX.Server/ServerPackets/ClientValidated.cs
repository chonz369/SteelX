using SteelX.Shared;
using SteelX.Server;

namespace SteelX.Server.Packets
{
	/// <summary>
	/// Sent to the client when they are validated?
	/// </summary>
	public class ClientValidated : ServerBasePacket
	{
		public override Shared.PacketTypes PacketType
		{
			get
			{
				return Shared.PacketTypes.CLIENT_VALIDATED;
			}
		}

		/*public override string GetType()
		{
			return "CLIENT_VALIDATED";
		}

		public override byte GetId()
		{
			return 0x01;
		}*/

		protected override void WriteImpl()
		{
			//WriteInt(1);
		}
	}
}