namespace SteelX.Shared
{
    /// <summary>
    /// Stats for a user 
    /// </summary>
    public class UserStats
    {
        /// <summary>
        /// The id of this stats entry
        /// </summary>
        public uint Id { get; set; }

        /// <summary>
        /// The user these stats belong to
        /// </summary>
        public Player User { get; set; }
        public uint UserId { get; set; }

        /// <summary>
        /// The type of stat this is
        /// </summary>
        public StatTypes Type { get; set; }

        // Stats go here
    }

    /// <summary>
    /// Used for both an individual player, and a Clan
    /// </summary>
    public class StatItemData
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
}