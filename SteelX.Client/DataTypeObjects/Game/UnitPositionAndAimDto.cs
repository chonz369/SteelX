using System;
using SteelX.Shared;
//using Data.Model;
//using GameServer.ServerPackets.Game;
//using Console = Colorful.Console;

namespace SteelX.Client.Packets.Game
{
	/// <summary>
	/// Base packet for game packets from the client
	/// </summary>
	[Serializable]
	public class UnitPositionAndAimDto
	{
		public float AimY { get; private set; }
		public float AimX { get; private set; }
		public float WorldPositionX { get; private set; }
		public float WorldPositionY { get; private set; }
		public float WorldPositionZ { get; private set; }
	}
}