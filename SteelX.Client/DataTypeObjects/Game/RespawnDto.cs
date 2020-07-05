using System;
using System.Linq;
using SteelX.Shared;
//using System.Drawing;
//using System.Numerics;
//using GameServer.ServerPackets.Game;
//using Console = Colorful.Console;

namespace SteelX.Client.Packets.Game
{
    /// <summary>
    /// Called when the user wishes to respawn
    /// </summary>
	[System.Serializable]
    public class RespawnDto
    {
        public uint _unitId { get; private set; }
    }
}