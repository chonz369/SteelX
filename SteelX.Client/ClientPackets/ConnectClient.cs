using System;
using System.Linq;
using SteelX.Shared;
//using Data.Model;
//using GameServer.Database;
//using GameServer.ServerPackets;
//using Microsoft.EntityFrameworkCore;
//using Console = Colorful.Console;

namespace SteelX.Client.Packets
{
	/// <summary>
	/// Called when the user logs in for the first time
	/// </summary>
	/// This is patched in our client to send unencrypted info, since we could not decrypt the original packet
	//TODO: Encrypt with different method for security?
	public class ConnectClient : ClientBasePacket
	{
		/// <summary>
		/// The user name
		/// </summary>
		private readonly string _userName;
		
		/// <summary>
		/// The password
		/// </summary>
		private readonly string _passWord;

		public override Shared.PacketTypes PacketType
		{
			get
			{
				return Shared.PacketTypes.CONNECT_CLIENT;
			}
		}

		public ConnectClient(byte[] data, GameSession client) : base(data, client)
		{
			_userName = GetString().Trim();
			_passWord = GetString().Trim();
		}

		/*public override string GetType()
		{
			return "CONNECT_CLIENT";
		}*/

		protected override void RunImpl()
		{
			GetClient().SendPacket(TryLogin());
		}

		/// <summary>
		/// Attempts to log in and returns the correct packet
		/// </summary>
		/// <returns></returns>
		private ConnectResult TryLogin()
		{
			using (var db = new ExteelContext())
			{               
				// Find user
				var user = db.Users.SingleOrDefault(u => u.Username == _userName);
				
				// If not user?
				if (user == null)
					return new ConnectResult(1);
				
				if (user.Password != _passWord)
					return new ConnectResult(2);
				
				//TODO: Check for ban state / admin state / user already logged in
				
				// Successful login from here, so assign user
				
				// Load user inventory
				user.Inventory = db.Inventories
					.Include(i => i.Parts)
					
					.Include(i => i.Units)
						.ThenInclude(u => u.Head)
					
					.Include(i => i.Units)
						.ThenInclude(u => u.Chest)
					
					.Include(i => i.Units)
						.ThenInclude(u => u.Arms)
					
					.Include(i => i.Units)
						.ThenInclude(u => u.Legs)
					
					.Include(i => i.Units)
						.ThenInclude(u => u.Backpack)
					
					.Include(i => i.Units)
						.ThenInclude(u => u.WeaponSet1Left)
					
					.Include(i => i.Units)
						.ThenInclude(u => u.WeaponSet1Right)
					
					.Include(i => i.Units)
						.ThenInclude(u => u.WeaponSet2Left)
					
					.Include(i => i.Units)
						.ThenInclude(u => u.WeaponSet2Right)
					
					.SingleOrDefault(i => i.Id == user.InventoryId);
				
				//TODO: Better error checking and logging
				if (user.Inventory == null)
				{
					Console.WriteLine("Unable to find user's inventory!");
					return new ConnectResult(1);
				}
				
				// Assign user units
				//TODO: Maybe we can map this, not sure
				user.Inventory.Units.ForEach(u => u.User = user);
				
				
				// Load users stats
				db
					.Entry(user)
					.Collection(u => u.Stats)
					.Load();
				
				// Assign to client
				GetClient().User = user;
				
				if (string.IsNullOrEmpty(user.Callsign))
					return new ConnectResult(-6, user);
				
				return new ConnectResult(0, user);
			}
		}
	}
}