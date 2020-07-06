using System;
using SteelX.Shared;
//using System.Drawing;
//using GameServer.ServerPackets;
//using Console = Colorful.Console;

namespace SteelX.Client.Packets
{
	/// <summary>
	/// Packet when user reports their protocol version
	/// </summary>
	[System.Serializable]
	public class ProtocolVersionDto
	{
		/// <summary>
		/// The version of protocol the client uses
		/// </summary>
		/// Currently is 333
		public int _version { get; private set; }
	}
}