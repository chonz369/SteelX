using System;
using System.Linq;
using SteelX.Shared;
//using System.Drawing;
//using Data.Model;
//using Console = Colorful.Console;

namespace SteelX.Client.Packets.Game
{
	/// <summary>
	/// Sent when the user stops aiming at a unit
	/// </summary>
	[System.Serializable]
	public class UnAimUnitDto
	{
		public Mechanaught _oldTarget { get; private set; }
	}
}