using System.Collections.Generic;
using SteelX.Shared;

namespace SteelX.Server
{
	public class Player : SteelX.Shared.Player
	{
		#region Variables
		/// <summary>
		/// Unique user Id for this user.
		/// </summary>
		/// Not sure if it is globally unique or just for the same session
		//public System.Guid Uid { get; private set; }
		public uint Id { get; protected set; }

		/// <summary>
		/// Possible remnant of social system. Not used in game so far in favor of callsign
		/// </summary>
		//public string Nickname { get; set; }

		/// <summary>
		/// Username the user uses to sign into their account
		/// </summary>
		/// Not used in game so far
		//public string Username { get; set; }

		/// <summary>
		/// You will give your pilot a Call Sign. 
		/// This name is permanent and cannot be changed. 
		/// You can only have one pilot per account, 
		/// and that pilot will gain experience and ranks over time.
		/// Callsign the user selected on first sign in. This is used as their display name
		/// </summary>
		//public string PilotName { get; private set; }
		public string CallSign { get; protected set; }

		/// <summary>
		/// The level of this user
		/// </summary>
		public uint Level { get; protected set; }

		/// <summary>
		/// The rank of this user
		/// </summary>
		public Ranks Rank { get; }

		/// <summary>
		/// The exp points of this user
		/// </summary>
		/// NOTE: Have not been able to see this reflected in game yet
		public uint Experience { get; protected set; }

		/// <summary>
		/// The amount of credits this user has
		/// </summary>
		public uint Credits { get; protected set; }

		/// <summary>
		/// The amount of NcCoins this user has
		/// </summary>
		public uint Coins { get; protected set; }

		/// <summary>
		/// The clan this user is in, if relevant
		/// </summary>
		//public Clan Clan { get; set; }
		public uint? ClanId { get; protected set; }

		/// <summary>
		/// The pilot groth info object for this user
		/// </summary>
		//ToDo: Rename to `Operator`
		public Pilot PilotInfo { get; protected set; }

		/// <summary>
		/// This user's inventory
		/// </summary>
		public UserInventory Inventory { get; }
		
		/// <summary>
		/// </summary>
		/// Repair points feed from inventory count for repair item
		//public int RepairPoints { get; }

		/// <summary>
		/// All this users stats
		/// </summary>
		//int[] Stats/Experience Points //Feed From Server, may not need it in class
		//public List<UserStats> Stats { get; set; }
		public int[] Stats { get; set; }

		/// <summary>
		/// Slot Id for User's Active Mech in Hanger Inventory
		/// </summary>
		public uint InventoryId { get; protected set; }

		/// <summary>
		/// The default unit for this user
		/// </summary>
		//TODO: Update this when they change default unit
		//public MechData ActiveMech { get; }
		public Mechanaught DefaultUnit { get; }
		public long DefaultUnitId { get; protected set; }

		/// <summary>
		/// The room this user is currently in
		/// </summary>
		//public RoomInfo Room { get; set; }
		//public uint RoomId { get; set; }

		/// <summary>
		/// The team this user is on
		/// </summary>
		/// Only used at runtime
		public byte Team { get; protected set; }

		/// <summary>
		/// Is the user ready
		/// </summary>
		/// Only used at runtime
		public bool IsReady { get; protected set; }

		/// <summary>
		/// The current unit this user is controlling
		/// </summary>
		/// Only used in game
		//ToDo: Use InventoryId?
		public long CurrentUnit { get; protected set; }
		#endregion

		//Mechs Array[SlotsAvailable] LoadOuts
		//byte LoadoutSlots

		#region Constructor
		#endregion

		#region Methods
		#endregion
	}
}