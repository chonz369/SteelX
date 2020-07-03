using SteelX.Shared;

namespace SteelX.Server.Packets.Game
{
	/// <summary>
	/// Sent when all units are loaded in and the game is ready to begin
	/// </summary>
	public class GameStarted : ServerBasePacket
	{
		public override Shared.PacketTypes PacketType
		{
			get
			{
				return Shared.PacketTypes.GAME_STARTED;
			}
		}

		/*public override string GetType()
		{
			return "GAME_STARTED";
		}

		public override byte GetId()
		{
			return 0x4e;
		}*/

		protected override void WriteImpl()
		{
			WriteInt(0); // Unknown
			WriteInt(0); // Unknown
		}
	}
}