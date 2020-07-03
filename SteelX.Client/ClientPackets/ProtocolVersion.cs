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
	public class ProtocolVersion : ClientBasePacket
	{
		/// <summary>
		/// The version of protocol the client uses
		/// </summary>
		/// Currently is 333
		private readonly int _version;

		public override Shared.PacketTypes PacketType
		{
			get
			{
				return Shared.PacketTypes.VERSION;
			}
		}

		public ProtocolVersion(byte[] data, GameSession client) : base(data, client)
		{
			try
			{
				_version = GetInt();
			}
			catch (Exception e)
			{
				System.Console.WriteLine("Error on client protocol version!");
				_version = 0;
				client.Disconnect();
			}
		}

		protected override void RunImpl()
		{
			//TODO: Check version?
			
			// Set on client
			GetClient().Version = _version;
			
			//TODO: If debug
			Console.WriteLine("Client protocol is : {0}", _version, Color.DodgerBlue);
			
			// Send the client their version
			GetClient().SendPacket(new ServerVersion());
		}
		
		/*public override string GetType()
		{
			return "VERSION";
		}*/
	}
}