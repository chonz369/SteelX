using System;
using System.Linq;
using SteelX.Shared;
//using System.Drawing;
//using GameServer.Managers;
//using GameServer.ServerPackets;
//using Console = Colorful.Console;

namespace SteelX.Client.Packets
{
	/// <summary>
	/// Sent when the client re-connects after switching to a room server
	/// </summary>
	[System.Serializable]
	public class ConnectSwitchDto
	{
		/// <summary>
		/// The job code used to reconnect
		/// </summary>
		//TODO: Error handling on all this
		public int _jobCode { get; private set; }
	}
}