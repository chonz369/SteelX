using System;
using SteelX.Shared;
//using System.Drawing;
//using System.Linq;
//using Console = Colorful.Console;

namespace SteelX.Client.Packets
{
	/// <summary>
	/// Dummy packet for unknown client packets
	/// </summary>
	public class UnknownPacket : ClientBasePacket
	{
		public override Shared.PacketTypes PacketType
		{
			get
			{
				return Shared.PacketTypes.UNKNOWN;
			}
		}

		public UnknownPacket(byte[] data, GameSession client) : base(data, client)
		{
			Console.WriteLine("Packet size: {0}",Color.Coral, Size);
			
			Console.WriteLine("Packet raw: {0}", Color.Coral,
				String.Join(" - ", _raw.Select(b => b.ToString("X2")).ToArray()));
		}

		/*public override string GetType()
		{
			return "UNKNOWN";
		}*/

		protected override void RunImpl()
		{
			// Do nothing
		}
	}
}