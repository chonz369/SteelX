using System;
using System.Linq;
using SteelX.Shared;
//using System.Drawing;
//using Console = Colorful.Console;

namespace SteelX.Client.Packets.Game
{
	/// <summary>
	/// Sent when a user has sucessfully loaded into the game and is ready to play
	/// </summary>
	public class ReadyGame : ClientBasePacket
	{
		public override Shared.PacketTypes PacketType
		{
			get
			{
				return Shared.PacketTypes.GAME_READY_GAME;
			}
		}

		public ReadyGame(byte[] data, GameSession client) : base(data, client)
		{
			Console.WriteLine("Packet size: {0}",Color.Coral, Size);
			
			Console.WriteLine("Packet raw: {0}", Color.Coral,
				String.Join(" - ", _raw.Select(b => b.ToString("X2")).ToArray()));

			Console.WriteLine("Int?? - : {0}", GetInt()); // ??
		}

		/*public override string GetType()
		{
			return "GAME_READY_GAME";
		}*/

		protected override void RunImpl()
		{
			var client = GetClient();

			client.GameInstance.GameReady(client);
		}
	}
}