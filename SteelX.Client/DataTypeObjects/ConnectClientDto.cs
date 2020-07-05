using System;
using System.Linq;
using SteelX.Shared;
//using Data.Model;
//using GameServer.Database;
//using GameServer.ServerPackets;
//using Microsoft.EntityFrameworkCore;
//using Console = Colorful.Console;

namespace SteelX.Client.Packets
{
	/// <summary>
	/// Called when the user logs in for the first time
	/// </summary>
	/// This is patched in our client to send unencrypted info, since we could not decrypt the original packet
	//TODO: Encrypt with different method for security?
	[System.Serializable]
	public class ConnectClientDto
	{
		/// <summary>
		/// The user name
		/// </summary>
		public string _userName { get; private set; }
		
		/// <summary>
		/// The password
		/// </summary>
		public string _passWord { get; private set; }
	}
}