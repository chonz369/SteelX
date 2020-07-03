using SteelX.Shared;

namespace SteelX.Server.Packets
{
	/// <summary>
	/// The server sends its version as a response to the client
	/// </summary>
	public class ServerVersion : ServerBasePacket
	{
		public override Shared.PacketTypes PacketType
		{
			get
			{
				return Shared.PacketTypes.SERVER_VERSION;
			}
		}

		/*public override string GetType()
		{
			return "SERVER_VERSION";
		}
		
		public override byte GetId()
		{
			return 0x00;
		}*/

		protected override void WriteImpl()
		{
			WriteInt(0);
			WriteInt(0);
			WriteUInt(3459107938);
			WriteInt(0);
		}
	}
}