using System;
using SteelX.Shared;
//using System.Drawing;
//using Colorful;

namespace SteelX.Client.Packets
{
	/// <summary>
	/// Logging packet from client
	/// </summary>
	public class Log : ClientBasePacket
	{
		/// <summary>
		/// Log data client sent us
		/// </summary>
		private readonly string _logString;

		public override Shared.PacketTypes PacketType
		{
			get
			{
				return Shared.PacketTypes.LOG;
			}
		}
		
		public Log(byte[] data, GameSession client) : base(data, client)
		{
			_logString = GetString();
		}

		/*public override string GetType()
		{
			return "LOG";
		}*/

		protected override void RunImpl()
		{
			//TODO: If config C_LOG
			//Console.WriteLine(_logString, Color.DodgerBlue);
		}
	}
}