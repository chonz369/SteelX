using System;
using SteelX.Shared;
//using GameServer.ServerPackets;

namespace SteelX.Client.Packets
{
	/// <summary>
	/// Sent when the client wishes to select their operator
	/// </summary>
	[System.Serializable]
	public class SelectOperatorDto
	{
		/// <summary>
		/// The operator they wish to select
		/// </summary>
		public uint _operatorId { get; private set; }
	}
}