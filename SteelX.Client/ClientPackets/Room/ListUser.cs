using System;
using System.Linq;
using System.Collections.Generic;
using SteelX.Shared;
//using Data.Model;
//using GameServer.ServerPackets.Room;
//using Microsoft.EntityFrameworkCore;
//using Console = Colorful.Console;

namespace SteelX.Client.Packets.Room
{
	/// <summary>
	/// Called when the user enters a room and wants a list of the current users
	/// </summary>
	public class ListUser : ClientBasePacket
	{
		public override Shared.PacketTypes PacketType
		{
			get
			{
				return Shared.PacketTypes.ROOM_LIST_USER;
			}
		}

		public ListUser(byte[] data, GameSession client) : base(data, client)
		{
		}

		/*public override string GetType()
		{
			return "ROOM_LIST_USER";
		}*/

		protected override void RunImpl()
		{
			var room = GetClient().GameInstance;
			//List<Player> users;
			//RoomInfo room;
			
			// Get the users in the room the user has joined
			//TODO: Update user and room in database to indicate they joined
			//using (var db = new ExteelContext())
			//{
			//	users = db.Users
			//		.Include(u => u.DefaultUnit)
			//		.ThenInclude(u => u.Head)
			//
			//		.Include(u => u.DefaultUnit)
			//		.ThenInclude(u => u.Chest)
			//
			//		.Include(u => u.DefaultUnit)
			//		.ThenInclude(u => u.Arms)
			//
			//		.Include(u => u.DefaultUnit)
			//		.ThenInclude(u => u.Legs)
			//		
			//		.Include(i => i.DefaultUnit)
			//		.ThenInclude(u => u.Backpack)
			//
			//		.Include(u => u.DefaultUnit)
			//		.ThenInclude(u => u.WeaponSet1Left)
			//
			//		.Include(u => u.DefaultUnit)
			//		.ThenInclude(u => u.WeaponSet1Right)
			//
			//		.Include(u => u.DefaultUnit)
			//		.ThenInclude(u => u.WeaponSet2Left)
			//
			//		.Include(u => u.DefaultUnit)
			//		.ThenInclude(u => u.WeaponSet2Right)
			//
			//		.Where(u => u.Room.Id == roomId).ToList();
			//
			//	room = db.Rooms.SingleOrDefault(r => r.Id == roomId);
			//}

			if (room.Users.Count == 0 || room == null)
			{
				Console.WriteLine("ERROR FINDINGS USERS IN ROOM!");
				return;
			}
			
			foreach (var user in room.Users)
			{
				// Send user info
				GetClient().SendPacket(new UserInfo(room, user));
				
				// Send unit info
				GetClient().SendPacket(new UnitInfo(user, user.DefaultUnit));
			}
		}
	}
}