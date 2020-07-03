using SteelX.Shared;
//using GameServer.ServerPackets.Room;

namespace SteelX.Client.Packets.Room
{
	/// <summary>
	/// Sent when the user wishes to start the game
	/// </summary>
	public class StartGame : ClientBasePacket
	{
		public override Shared.PacketTypes PacketType
		{
			get
			{
				return Shared.PacketTypes.ROOM_START_GAME;
			}
		}

		public StartGame(byte[] data, GameSession client) : base(data, client)
		{
		}

		/*public override string GetType()
		{
			return "ROOM_START_GAME";
		}*/

		protected override void RunImpl()
		{
			// TODO: Check errors - users not ready, team balance, etc
			var client = GetClient();
			
			client.GameInstance.MulticastPacket(new GameStart(client.GameInstance, 0));
		}
	}
}