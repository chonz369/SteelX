using System;
//using Console = Colorful.Console;
//using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
//using System.Numerics;
using SteelX.Shared;
using SteelX.Shared.Utility;
using SteelX.Shared.Packets;
//using SteelX.Server;
//using SteelX.Server.Packets;
//using SteelX.Server.Packets.Game;
//using SteelX.Server.Packets.Lobby;
//using SteelX.Server.Packets.Room;

namespace SteelX.Shared.Game
{
	/// <summary>
	/// A single game instance;
	/// Handles all data for scoring, user updates, game events, etc
	/// </summary>
	public class GameInstance
	{
		#region Variables
		/// <summary>
		/// The unique Id of this room/game
		/// </summary>
		public uint Id { get; protected set; }
		
		/// <summary>
		/// Type of game this room is
		/// </summary>
		public GameTypes GameType { get; protected set; }

		/// <summary>
		/// Create a name for the room, or match, you are setting up.
		/// </summary>
		public string RoomName { get; protected set; }
		
		/// <summary>
		/// The master of this room
		/// </summary>
		//public Player Master { get { return Users.FirstOrDefault(u => u.Id == MasterId); } }
		
		/// <summary>
		/// The Id of the room's Administrator
		/// </summary>
		public uint MasterId { get; protected set; }
		
		/// <summary>
		/// The game template id for this game
		/// </summary>
		public uint GameTemplate { get; protected set; }
	   
		/// <summary>
		/// The max number of users who can be in this room
		/// </summary>
		public int Capacity { get; protected set; }

		/// <summary>
		/// Dictionary of all sessions in this room
		/// </summary>
		//private readonly ConcurrentDictionary<Guid, GameSession> _sessions = new ConcurrentDictionary<Guid, GameSession>();
		//private readonly Dictionary<Guid, GameSession> _sessions = new Dictionary<Guid, GameSession>();
		private Dictionary<Guid, GameSession> _sessions { get; set; }
		
		/// <summary>
		/// Dictionary of all units in this room
		/// </summary>
		//private readonly ConcurrentDictionary<uint, Unit> _units = new ConcurrentDictionary<uint, Unit>();
		//private readonly Dictionary<uint, Mechanaught> _units = new Dictionary<uint, Mechanaught>();
		private Dictionary<uint, Mechanaught> _units { get; set; }

		/// <summary>
		/// The users in this room
		/// </summary>
		public virtual List<Player> Users { get { return _sessions.Values.Select(s => s.User).ToList(); } }
		#endregion
		
		/*#region MESSAGING		
		/// <summary>
		/// Multicasts a packet to all users in this room
		/// </summary>
		/// <param name="packet"></param>
		public void MulticastPacket(ServerBasePacket packet, int count = 1)
		{
			byte[] data;
			if (count == 1)
				data = packet.Write();
			else
			{
				// TEMP
				var tData = new List<byte>();
				for (var i = 0; i < count; i++)
				{
					tData.AddRange(packet.Write());
				}

				data = tData.ToArray();
			}
			
			// Multicast data to all sessions
			foreach (var session in _sessions.Values)
				//session.SendAsync(data);
				session.SendPacket(data);
			
			//TODO: Add config here - if debug
			Console.WriteLine("[S] 0x{0:x2} {1} >>> room: {2}", Color.Green, packet.GetId(), packet.GetType(), Id);
		}		
		#endregion
		
		#region UTILITIES
		/// <summary>
		/// Gets a unit in the room by its id
		/// </summary>
		/// <param name="id"></param>
		/// <returns></returns>
		public Mechanaught GetUnitById(uint id)
		{
			//TODO: Error handling
			return _units[id];
		}		
		#endregion
		
		#region JOINING / LEAVING
		/// <summary>
		/// Gets the team for a new user
		/// If survival (deathmatch) each user is on their own team
		/// If coop, users all on team 0
		/// Otherwise, users are on either team 0 or team 1
		/// </summary>
		/// <returns></returns>
		private uint GetTeamForNewUser()
		{
			if (GameType == GameTypes.Survival)
			{
				int numSessions = _sessions.Count;

				return (uint)numSessions;
			}
			else if (GameType == GameTypes.DefensiveBattle)
			{
				return 0;
			}
			else
			{
				// Figure out which team has more users and add them to the other team
				int numUsersTeamZero = _sessions.Values.Select(s => s.User.Team).Count(t => t == 0);
				int numUsersTeamOne = _sessions.Values.Select(s => s.User.Team).Count(t => t == 1);

				// If more users on team zero, they go to team one
				// Otherwise, team zero
				return numUsersTeamZero > numUsersTeamOne ? (uint) 1 : (uint) 0;
			}
		}

		/// <summary>
		/// Attempts to join this game
		/// </summary>
		/// <param name="session">The session trying to join</param>
		/// <returns></returns>
		public GameEntered TryEnterGame(GameSession session)
		{
			// If game is full
			if (_sessions.Count >= Capacity)
			{
				return new GameEntered(GameEnteredResult.Full);
			}
			
			//TODO: Other checks, too early, banned, no units, etc
			
			//TODO: Joining in progress?
			
			// Success
			
			// Setup team
			session.User.Team = GetTeamForNewUser();
			
			// Reset ready flag
			session.User.IsReady = false;
			session.IsGameReady = false;
			
			// Announce user to room
			MulticastPacket(new UserEnter(this, (Server.Player)session.User));
			MulticastPacket(new Server.Packets.Room.UnitInfo((Server.Player)session.User, session.User.DefaultUnit));
			
			// Add to session list
			_sessions.TryAdd(session.Id, session);
			
			// Assign to the client
			session.GameInstance = this;
			
			return new GameEntered(GameEnteredResult.Success, this);
		}

		/// <summary>
		/// Removes a session from this room
		/// </summary>
		/// <param name="id"></param>
		public void RemoveSession(Guid id)
		{
			// Unregister session by Id
			_sessions.TryRemove(id, out GameSession temp);
		}

		/// <summary>
		/// Called when a user has loaded into the game
		/// </summary>
		/// <param name="session"></param>
		public void GameReady(GameSession session)
		{
			// Set flag to true
			session.IsGameReady = true;
			
			// Send user info
			MulticastPacket(new Server.Packets.Game.UserInfo(this, (Server.Player)session.User));
			
			// Load user stats here, so they are not loaded during gameplay
			session.User.DefaultUnit.CalculateStats();
			
			// Spawn user default unit
			SpawnUnit(session.User.DefaultUnit, session.User);
			
			// Check if everyone is ready, if so, start
			CheckAllUsersReady();
		}
		
		/// <summary>
		/// Checks if all users are ready and begins game if they are
		/// </summary>
		private void CheckAllUsersReady()
		{
			// If everyone is loaded in
			if (_sessions.Values.All(s => s.IsGameReady))
			{
				// Start threads
				foreach (var session in _sessions.Values)
				{
					//session.StartGameThread();
				}

				// Send game start
				MulticastPacket(new GameStarted());
			}
		}		
		#endregion
		
		#region UNITS
		/// <summary>
		/// Spawns a new unit into the game
		/// </summary>
		/// <param name="unit"></param>
		/// <param name="user"></param>
		public void SpawnUnit(Mechanaught unit, Player user)
		{
			//TODO: See if we need to be destroying and removing units in dict or not
			_units[unit.Id] = unit;
			
			// Spawns a unit into the game

			// Set the current unit to the user
			//TODO: Do we need this?
			user.CurrentUnit = unit;

			//TODO: Calculate HP
			unit.Health = unit.HP;
			
			// Reset death flag
			//unit.Alive = true;
			
			// Set unit Location
			//TODO: Spawn maps?
			unit.WorldPosition = new Vector(3000, -3000, 3000);
			
			//TODO: Users have multiple units, do we need to handle that here?
			// Send unit info
			MulticastPacket(new Server.Packets.Game.UnitInfo(user, unit));
			
			// Send spawn command
			MulticastPacket(new SpawnUnit(unit));
		}

		/// <summary>
		/// Updates a units position
		/// Broadcasts to all users
		/// </summary>
		/// Checks for death by location
		//TODO: Broadcast to only nearby players?
		public void UpdateUnitPosition(Mechanaught unit)
		{
			// Update user unit info
			MulticastPacket(new UnitMoved(unit));
			
			// Fall death
			//if (unit.WorldPosition.Z <= 2100 && unit.Alive)
			//{
			//    KillUnit(unit);
			//}
		}

		/// <summary>
		/// Kills a unit
		/// </summary>
		/// <param name="unit"></param>
		/// <param name="killer"></param>
		//TODO: Find out how to handle killer / victim packets
		public void KillUnit(Mechanaught unit, Mechanaught killer = null)
		{
			unit.Health = 0;
			//unit.Alive = false;
			
			MulticastPacket(new UnitDestroyed(unit, killer));
		}

		/// <summary>
		/// Updates users when a target has been made on a unit
		/// </summary>
		/// <param name="attacker">Attacker</param>
		/// <param name="target">Target</param>
		//TODO: Should this be multicasted or just to the two users?
		//TODO: Do we need to factor arm into this?
		public void AimUnit(Mechanaught attacker, Mechanaught target)
		{
			MulticastPacket(new AimUnit(attacker, target.Id));     
		}

		/// <summary>
		/// Updates users when a target is no longer being aimed at
		/// </summary>
		/// <param name="attacker"></param>
		/// <param name="oldTarget"></param>
		public void UnAimUnit(Mechanaught attacker, Mechanaught oldTarget)
		{
			var oldTargetId = oldTarget == null ? 0 : oldTarget.Id;
			System.Console.WriteLine("Un aimed - {0}", oldTargetId);
			MulticastPacket(new UnAimUnit(attacker, oldTarget.Id));
		}
		
		#region ATTACKS
		/// <summary>
		/// Trys to attack using a weapon
		/// </summary>
		/// <param name="attacker"></param>
		/// <param name="weapon"></param>
		public void TryAttack(Mechanaught attacker, int arm)
		{
			// Get weapon
			var weapon = attacker.GetWeaponByArm(arm);
			
			// Check overheat status
			//TODO: Do we need to send packet if this happens?
			if (weapon.IsOverheated) return;
			
			// If automatic, handle reload time
			if (weapon.IsAutomatic)
				weapon.AddReloadTime();
			
			// Update overheat
			//TODO: Handle cooldown...
			if (weapon.AddOverheat())
			{
				MulticastPacket(new OverheatStatus(attacker, arm, weapon));
			}
			
			// Send attack
			// Temp - can we group these???
			//for (var i=0; i <weapon.NumberOfShots; i++)
			MulticastPacket(new AttackStart(attacker, arm, weapon), 1);
			
			//TODO: Do we need to multicast this?
			//MulticastPacket(new OverheatStatus(attacker, arm, weapon));

			// If they missed, do nothing
			if (weapon.Target == null) return;
			
			// Check for kill
			weapon.Target.Health -= weapon.Damage*weapon.NumberOfShots;

			// Kill unit
			//TODO: Are more status updates needed?
			if (weapon.Target.Health <= 0)
			{
				KillUnit(weapon.Target, attacker);
			}
		}		
		#endregion

		/// <summary>
		/// Called when a unit needs to be ticked to process status
		/// </summary>
		/// <param name="unit"></param>
		/// <param name="timeStamp"></param>
		public void TickUnit(Mechanaught unit, float delta)
		{           
			// Do ticks
			//TODO: Clean this up
			//Console.WriteLine("TICK - delta: {0}", delta);
			
			// Tick weapons
			//TODO: Tick unequipped weapons as well
			var weapon = unit.GetWeaponByArm(0);
			
			if (weapon.ShouldUpdateOverheat(delta))
			{
				MulticastPacket(new OverheatStatus(unit, 0, weapon));
			}
			
			// Check fire first
			if (weapon.ShouldAttack(delta))
			{
				TryAttack(unit, 0);
				// TEMP
				Console.WriteLine("GOT YES TO SHOULD ATTACK -- THIS SHOULD ONLY PRINT ONCE");
			}

			weapon = unit.GetWeaponByArm(1);
			
			if (weapon.ShouldUpdateOverheat(delta))
			{
				MulticastPacket(new OverheatStatus(unit, 1, weapon));
			}
			
			// Check fire first
			if (weapon.ShouldAttack(delta))
			{
				TryAttack(unit, 1);
				// TEMP
				Console.WriteLine("GOT YES TO SHOULD ATTACK -- THIS SHOULD ONLY PRINT ONCE");
			}
		}		
		#endregion*/
	}
}