using SteelX.Shared;

namespace SteelX.Server.Game
{
	public class GameInfo
	{
		/// <summary>
		/// Unique Id for this game session.
		/// Have not found this in the protocol, but i am guess it is in there somewhere
		/// </summary>
		public uint Id;
		
		/// <summary>
		/// The game mode of this game
		/// </summary>
		public GameModes GameMode;

		/// <summary>
		/// The name of this game - shown in lobby
		/// </summary>
		public string GameName;

		/// <summary>
		/// Seems to be a string in the protocol, maybe for password?
		/// </summary>
		public bool IsPrivate;

		/// <summary>
		/// The master of this game
		/// </summary>
		public Player GameMaster;

		/// <summary>
		/// The maximum users for this game
		/// Possibly display only
		/// </summary>
		public uint MaxUsers;

		/// <summary>
		/// The current users in this game
		/// Possibly display only
		/// </summary>
		public uint CurrentUsers;

		/// <summary>
		/// Used to control the map, and also the room info?
		/// </summary>
		public uint GameTemplateId;
	}
}