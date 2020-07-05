using System;
using System.Linq;
using SteelX.Shared;
//using System.Drawing;
//using GameServer.ServerPackets.Game;
//using Console = Colorful.Console;

namespace SteelX.Client.Packets.Game
{
	/// <summary>
	/// Sent frequently by the user to indicate their current position
	/// </summary>
	[System.Serializable]
	public class MoveUnitDto
	{
		public byte Boosting			{ get; private set; }
		public byte Movement			{ get; private set; }
		public byte UnknownMovementFlag { get; private set; }
	}
}