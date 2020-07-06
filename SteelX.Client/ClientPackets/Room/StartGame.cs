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
			
			//Send a request to start game match 
			//if all users are ready, server will respond with `OK` to all
			//if game cannot begin, server will let host know, and send message to chat
			
			//client.GameInstance.MulticastPacket(new GameStart(client.GameInstance, 0));
		}
	}
}