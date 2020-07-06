using SteelX.Shared;
using System.Collections.Generic;

namespace SteelX.Server
{
	/// <summary>
	/// A users inventory
	/// </summary>
	public class UserInventory
	{
		/// <summary>
		/// Unique Id for this inventory.
		/// </summary>
		/// Primary Key
		public uint Id { get; private set; }
		
		/// <summary>
		/// The use who owns this inventory
		/// </summary>
		/// Foreign Key
		public uint UserId { get; private set; }
		
		/// <summary>
		/// The use who owns this inventory
		/// </summary>
		//public User User { get { return Application.UserData; } }
		
		/// <summary>
		/// This user's units
		/// </summary>
		public ICollection<Mechanaught> Units { get; private set; }

		/// <summary>
		/// This users parts
		/// </summary>
		public ICollection<Part> Parts { get; private set; }

		/// <summary>
		/// Number of unit slots this user has
		/// </summary>
		public uint UnitSlots { get; private set; }

		/// <summary>
		/// The maximum size of this users inventory
		/// </summary>
		public uint InventorySize { get; private set; }

		/// <summary>
		/// The current number of items in the users inventory
		/// </summary>
		public int InventoryUsed => Parts.Count;
	}

	/// <summary>
	/// A users item in inventory
	/// </summary>
	public class UserInventoryItem
	{
		/// <summary>
		/// Unique Id for this inventory.
		/// </summary>
		/// Primary Key
		public uint Id { get; private set; }
		
		/// <summary>
		/// The use who owns this inventory
		/// </summary>
		/// Foreign Key
		public uint UserId { get; private set; }
		
		/// <summary>
		/// The user who owns this inventory
		/// </summary>
		/// Foreign Key
		public uint ItemId { get; private set; }
		
		/// <summary>
		/// The use who owns this inventory
		/// </summary>
		//public User User { get { return Application.UserData; } }
		
		/// <summary>
		/// This user's units
		/// </summary>
		public ICollection<Mechanaught> Units { get; private set; }

		/// <summary>
		/// This users parts
		/// </summary>
		public ICollection<Part> Parts { get; private set; }

		/// <summary>
		/// Number of unit slots this user has
		/// </summary>
		public uint UnitSlots { get; private set; }

		/// <summary>
		/// The maximum size of this users inventory
		/// </summary>
		public uint InventorySize { get; private set; }

		/// <summary>
		/// The current number of items in the users inventory
		/// </summary>
		public int InventoryUsed => Parts.Count;
	}

	/// <summary>
	/// A users Mech in Hanger
	/// </summary>
	public class UserInventoryMech
	{
		/// <summary>
		/// Unique Id for this inventory.
		/// </summary>
		/// Primary Key
		public uint Id { get; private set; }
		
		/// <summary>
		/// The user who owns this inventory
		/// </summary>
		/// Foreign Key
		public uint UserId { get; private set; }
		
		/// <summary>
		/// The order or position of this particular mech suit
		/// </summary>
		/// Foreign Key
		public uint Slot { get; private set; }
		
		/// <summary>
		/// This user's units
		/// </summary>
		public ICollection<Mechanaught> Units { get; private set; }

		/// <summary>
		/// This users parts
		/// </summary>
		/// using array of item ids from inventory
		//public uint[] ItemId { get; private set; }
		public ICollection<Part> Parts { get; private set; }
	}
}