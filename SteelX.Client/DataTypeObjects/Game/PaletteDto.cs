using System;
using System.Linq;
using SteelX.Shared;
//using GameServer.ServerPackets.Game;

namespace SteelX.Client.Packets.Game
{
	/// <summary>
	/// Seems to be sent by client to request palettes for units
	/// </summary>
	/// Maybe for preloading skills?
	/// Or showing in bar?
	/// Assuming palette is skills currently
	[System.Serializable]
	public class PaletteDto
	{
		/// <summary>
		/// The unit ID being requested
		/// </summary>
		public uint _unitId { get; private set; }
	}
}