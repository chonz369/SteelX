using SteelX.Shared;

namespace SteelX.Server.Packets.Game
{
	/// <summary>
	/// Sent as a reponse to users when they select a base
	/// </summary>
	/// Not fully understood
	public class BaseSelected : ServerBasePacket
	{
		public override Shared.PacketTypes PacketType
		{
			get
			{
				return Shared.PacketTypes.GAME_BASE_SELECTED;
			}
		}

		/*public override string GetType()
		{
			return "GAME_BASE_SELECTED";
		}

		public override byte GetId()
		{
			return 0x55;
		}*/

		protected override void WriteImpl()
		{
			WriteInt(0); // Unknown
			WriteInt(0); // Unknown
		}
	}
}