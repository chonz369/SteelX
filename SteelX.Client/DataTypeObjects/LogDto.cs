using System;
using SteelX.Shared;
//using System.Drawing;
//using Colorful;

namespace SteelX.Client.Packets
{
	/// <summary>
	/// Logging packet from client
	/// </summary>
	[System.Serializable]
	public class LogDto
	{
		/// <summary>
		/// Log data client sent us
		/// </summary>
		public string _logString { get; private set; }
	}
}