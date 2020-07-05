using SteelX.Shared;
//using GameServer.ServerPackets.Room;

namespace SteelX.Client.Packets.Room
{
	/// <summary>
	/// Sent when a user toggles their ready status
	/// </summary>
	[System.Serializable]
	public class ReadyDto
	{
		/// <summary>
		/// Are they ready
		/// </summary>
		public bool _ready { get; private set; }
	}
}