namespace SteelX.Shared
{
	/// <summary>
	/// Used for both an individual player, and a Clan
	/// </summary>
	public class StatData
	{
		/// <summary>
		/// The id of this stats entry
		/// </summary>
		public uint Id { get; private set; }

		/// <summary>
		/// The user or clan these stats belong to
		/// </summary>
		public uint ObjectId { get; private set; }

		/// <summary>
		/// The type of stat this is
		/// </summary>
		public StatTypes Type { get; private set; }
		public GameTypes Mode { get; private set; }
		//public uint MapId { get; private set; }        
	}

	/// <summary>
	/// The type of stat this is
	/// Guessing: DM, TDM, TC, etc
	/// </summary>
	public enum StatTypes : byte
	{
		Training = 0,
		Survival = 1,
		TeamSurvival = 2,
		TeamBattle = 3,
		ClanBattle = 4,
		DefensiveBattle = 5
	}
}