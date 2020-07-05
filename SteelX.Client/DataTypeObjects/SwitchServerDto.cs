using System;
using System.Linq;
using SteelX.Shared;
//using System.Drawing;
//using GameServer.Managers;
//using Console = Colorful.Console;

namespace SteelX.Client.Packets
{
	/// <summary>
	/// Sent when the user wants to switch to a room server
	/// </summary>
	[System.Serializable]
	public class SwitchServerDto
	{
		/// <summary>
		/// </summary>
		/// Not sure how this is used yet
		/// Looks to be server id string?
		public string _serverId { get; private set; }
	}
}