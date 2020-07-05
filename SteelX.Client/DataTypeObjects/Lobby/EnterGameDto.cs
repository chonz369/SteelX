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
	[System.Serializable]
	public class EnterGameDto
	{
		/// <summary>
		/// The room Id in number form
		/// </summary>
		public int _roomId { get; private set; }
	}
}