using SteelX.Shared;

namespace SteelX.Client.Packets.Game
{
	/// <summary>
	/// Sent when the user stops attacking with an automatic weapon
	/// </summary>
	[System.Serializable]
	public class StopAttackDto
	{
		/// <summary>
		/// The arm they are using
		/// </summary>
		public int _arm { get; private set; }
	}
}