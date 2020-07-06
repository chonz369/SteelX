using System;
using System.Linq;
using SteelX.Shared;
//using System.Drawing;
//using GameServer.ServerPackets.Game;
//using Console = Colorful.Console;

namespace SteelX.Client.Packets.Game
{
	/// <summary>
	/// Sent when the user first spawns.
	/// </summary>
	/// Not sure exactly how it works
	public class SelectBase : ClientBasePacket
	{
		public override Shared.PacketTypes PacketType
		{
			get
			{
				return Shared.PacketTypes.GAME_SELECT_BASE;
			}
		}

		public SelectBase(byte[] data, GameSession client) : base(data, client)
		{
			/*Console.WriteLine("Packet size: {0}",Color.Coral, Size);
			
			Console.WriteLine("Packet raw: {0}", Color.Coral,
				String.Join(" - ", _raw.Select(b => b.ToString("X2")).ToArray()));

			Console.WriteLine("Int?? - : {0}", GetInt()); // ??*/
		}

		/*public override string GetType()
		{
			return "GAME_SELECT_BASE";
		}*/

		protected override void RunImpl()
		{
			//GetClient().SendPacket(new BaseSelected());

			//Client sends a message to server
			//Decides which spawn point to use to initialize end user
		}
	}
}