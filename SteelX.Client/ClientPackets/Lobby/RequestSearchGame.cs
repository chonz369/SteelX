using System;
using System.Linq;
using SteelX.Shared;
//using System.Drawing;
//using GameServer.Managers;
//using GameServer.ServerPackets.Lobby;
//using Console = Colorful.Console;

namespace SteelX.Client.Packets.Lobby
{
	/// <summary>
	/// Called when the user refreshes their lobby games list
	/// </summary>
	public class RequestSearchGame : ClientBasePacket
	{
		public override Shared.PacketTypes PacketType
		{
			get
			{
				return Shared.PacketTypes.LOBBY_REQ_SEARCH_GAME;
			}
		}

		public RequestSearchGame(byte[] data, GameSession client) : base(data, client)
		{
			Console.WriteLine("Packet size: {0}",Color.Coral, Size);
			
			Console.WriteLine("Packet raw: {0}", Color.Coral,
				String.Join(" - ", _raw.Select(b => b.ToString("X2")).ToArray()));

			
			Console.WriteLine("Int?? - : {0}", GetInt()); // ??
			Console.WriteLine("Int?? - : {0}", GetInt()); // ??
		}

		/*public override string GetType()
		{
			return "LOBBY_REQ_SEARCH_GAME";
		}*/

		protected override void RunImpl()
		{
			var rooms = RoomManager.GetRooms();
			// Search for rooms
			//TODO: Check the paramaters in this packet to see if they are filters or something
			
			GetClient().SendPacket(new GameSearched(rooms));
		}
	}
}