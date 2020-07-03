using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using SteelX.Shared;

namespace SteelX.Server
{
	public class GameServer //: TcpServer
	{
		#region Variables
		/// <summary>
		/// All online players currently logged on active server
		/// </summary>
		//public  HashSet<User> PlayerList { get; set; }
		public HashSet<User> Users { get; set; }

		/// <summary>
		/// Pilot info for each user (weapon and ability progression)
		/// </summary>
		//public DbSet<PilotInfo> PilotInfos { get; set; }

		/// <summary>
		/// Stats for all users
		/// </summary>
		public DbSet<UserStats> UserStats { get; set; }

		/// <summary>
		/// All user inventories (items)
		/// </summary>
		public DbSet<UserInventory> Inventories { get; set; }

		/// <summary>
		/// All parts in the system
		/// </summary>
		public HashSet<Part> Parts { get; set; }

		/// <summary>
		/// All units in the system
		/// </summary>
		public HashSet<Mechanaught> Units { get; set; }

		/// <summary>
		/// All clans in the system
		/// </summary>
		public HashSet<Clan> Clans { get; set; }

		/// <summary>
		/// The currently active rooms on the system
		/// </summary>
		//public DbSet<RoomInfo> Rooms { get; set; }
		#endregion

		#region Constructor
		private GameServer()
		{
			//PlayerList = new HashSet<User>();
		}

		/*public GameServer()
		{
		}

		public GameServer(IPAddress address, int port) : base(address, port)
		{
			//using (var db = new ExteelContext())
			//{
			//	db.Database.EnsureDeleted();
			//	db.Database.EnsureCreated();
			//}
		}*/
		#endregion

		#region Methods
		/*protected override TcpSession CreateSession()
		{
			var session = new GameSession(this);
			return session;
		}

		protected override void OnError(SocketError error)
		{
			Console.WriteLine($"Game TCP server caught an error with code {error}");
		}*/
		#endregion
	}
}