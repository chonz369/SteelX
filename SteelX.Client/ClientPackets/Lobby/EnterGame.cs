using System;
using System.Linq;
using SteelX.Shared;
//using System.Drawing;
//using Data.Model;
//using GameServer.Managers;
//using GameServer.ServerPackets.Lobby;
//using Console = Colorful.Console;

namespace SteelX.Client.Packets.Lobby
{
	/// <summary>
	/// Called when a user enters a game after server switch
	/// </summary>
	public class EnterGame : ClientBasePacket
	{
		public override Shared.PacketTypes PacketType
		{
			get
			{
				return Shared.PacketTypes.LOBBY_ENTER_GAME;
			}
		}

		/// <summary>
		/// The room Id in number form
		/// </summary>
		private readonly int _roomId;
		
		public EnterGame(byte[] data, GameSession client) : base(data, client)
		{
			Console.WriteLine("Packet size: {0}",Color.Coral, Size);
			
			Console.WriteLine("Packet raw: {0}", Color.Coral,
				String.Join(" - ", _raw.Select(b => b.ToString("X2")).ToArray()));

			_roomId = GetInt();
			Console.WriteLine("Int?? - : {0}", _roomId); // ??
			Console.WriteLine("Int?? - : {0}", GetInt()); // ??
		}

		/*public override string GetType()
		{
			return "LOBBY_ENTER_GAME";
		}*/

		protected override void RunImpl()
		{
			var room = RoomManager.GetRoomById(_roomId);
			var client = GetClient();
			
			GetClient().SendPacket(room.TryEnterGame(client));
		}
	}
}