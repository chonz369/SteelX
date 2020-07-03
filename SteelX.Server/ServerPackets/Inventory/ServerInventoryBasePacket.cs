using SteelX.Shared;
using System.Linq;
using SteelX.Server;
//using SteelX.Server.Items;
//using Microsoft.EntityFrameworkCore;

namespace SteelX.Server.Packets.Inventory
{
	/// <summary>
	/// Base packet for all inventory packets
	/// </summary>
	public abstract class ServerInventoryBasePacket : ServerBasePacket
	{
		/// <summary>
		/// The inventory for the user
		/// </summary>
		protected readonly UserInventory Inventory;
		
		public ServerInventoryBasePacket(Player user)
		{
			// Load the users inventory
			Inventory = user.Inventory;
		}
	}
}