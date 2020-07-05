using System;
using SteelX.Shared;

namespace SteelX.Client.Packets
{
	/// <summary>
	/// Client packet to request the server time
	/// </summary>
	[System.Serializable]
	public class ServerTimeDto
	{
		/// <summary>
		/// Time client has been connected in MS
		/// </summary>
		/// I Think this is a DateTime represented as an int
		public uint _clientTime { get; private set; }
	}
}