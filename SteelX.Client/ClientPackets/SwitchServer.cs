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
	/// Should be the same thing as joining a room...
	/// you cant access a room from any other point except lobby
	//ToDo: Move Class/File to `Room` Directory, as it's function is geared to accessing rooms
	public class SwitchServer : ClientBasePacket
	{
		/// <summary>
		/// Not sure how this is used yet
		/// Looks to be server id string?
		/// </summary>
		private readonly string _serverId;

		public override Shared.PacketTypes PacketType
		{
			get
			{
				return Shared.PacketTypes.SWITCH_SERVER;
			}
		}

		public SwitchServer(byte[] data, GameSession client) : base(data, client)
		{
			//Console.WriteLine("Packet size: {0}",Color.Coral, Size);
			//
			//Console.WriteLine("Packet raw: {0}", Color.Coral,
			//	String.Join(" - ", _raw.Select(b => b.ToString("X2")).ToArray()));
			//
			//_serverId = GetString();
			//Console.WriteLine("Server Id? - : {0}", _serverId); // ??
			//Console.WriteLine("Int?? - : {0}", GetInt()); // ??
		}

		/*public override string GetType()
		{
			return "SWITCH_SERVER";
		}*/

		protected override void RunImpl()
		{
			//Sends id of room user has selected to server
			//Server checks if room has availability or password, 
			//server replies back with confirmation or rejection
			//var jobCode = ServerManager.JoinServer(GetClient(), _serverId);
			//GetClient().SendPacket(new ServerPackets.SwitchServer(jobCode));
		}
	}
}