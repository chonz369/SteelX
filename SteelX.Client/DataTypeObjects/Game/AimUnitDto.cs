using System;
using System.Linq;
using SteelX.Shared;

namespace SteelX.Client.Packets.Game
{
	/// <summary>
	/// Sent when a user aims on a unit
	/// </summary>
	[Serializable]
	public class AimUnitDto 
	{
		public uint? Target { get; private set; }
		public bool Arm { get; private set; }

		//public Shared.PacketTypes PacketType
		//{
		//	get
		//	{
		//		return Shared.PacketTypes.AIM_UNIT;
		//	}
		//}
	}
}