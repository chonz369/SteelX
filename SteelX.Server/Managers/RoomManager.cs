using System;
using System.Collections.Generic;
using System.Linq;
using SteelX.Server;
using SteelX.Server.Game;
using SteelX.Server.Utility;

namespace SteelX.Server.Managers
{
	/// <summary>
	/// This class handles rooms, user switches, etc
	/// </summary>
	//TODO: See what needs to go into database and what can stay in memory
	public static class RoomManager
	{
		/// <summary>
		/// All current rooms
		/// </summary>
		private static readonly SortedList<int, GameInstance> Rooms = new SortedList<int, GameInstance>();

		/// <summary>
		/// Creates a new room and returns its ID
		/// </summary>
		/// <returns></returns>
		public static int CreateRoom(GameInstance room)
		{
			var id = Rooms.AddNext(room);

			room.Id = id;

			return id;
		}

		/// <summary>
		/// Deletes a room by its id
		/// </summary>
		/// <param name="id"></param>
		//TODO: Kick users back to menu?
		//TODO: Error handling
		public static void DeleteRoom(int id)
		{
			Rooms.Remove(id);
		}

		/// <summary>
		/// Finds a room by its id
		/// </summary>
		/// <param name="id"></param>
		/// <returns></returns>
		//TODO: Error handling
		public static GameInstance GetRoomById(int id)
		{
			return Rooms[id];
		}

		/// <summary>
		/// Gets all the rooms
		/// </summary>
		/// <returns></returns>
		//TODO: Filters?
		public static List<GameInstance> GetRooms()
		{
			return Rooms.Values.ToList();
		}
	}
}