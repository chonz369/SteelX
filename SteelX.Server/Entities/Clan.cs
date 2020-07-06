using System.Collections.Generic;

namespace SteelX.Server
{
	public class Clan
	{
		/// <summary>
		/// The unique Id of this clan
		/// </summary>
		public uint Id { get; private set; }
		
		/// <summary>
		/// The name of this clan
		/// </summary>
		public string Name { get; private set; }

		/// <summary>
		/// Clan banner
		/// </summary>
		public string Image { get; private set; }

		// Clan Leader
		// Clan Member Rankings
		// Clan Vanity Ranking
		// Clan Kill/Death/Win/Loss Match History

		/// <summary>
		/// List of users in this clan
		/// </summary>
		public HashSet<int> Players { get; private set; }
	}
}