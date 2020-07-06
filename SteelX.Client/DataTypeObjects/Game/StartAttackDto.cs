using System;
using System.Linq;
using SteelX.Shared;
//using System.Drawing;
//using GameServer.ServerPackets.Game;
//using Console = Colorful.Console;

namespace SteelX.Client.Packets.Game
{
	/// <summary>
	/// Sent when a user begins attacking
	/// </summary>
	[System.Serializable]
	public class StartAttackDto
	{
		/// <summary>
		/// The arm they are attacking with
		/// </summary>
		public int _arm { get; private set; }
		
		public int _comboStep { get; private set; }
	}
}