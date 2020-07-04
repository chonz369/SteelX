using System;
using System.Linq;
using System.Net.Sockets;
//using System.Threading.Tasks;
using SteelX.Shared;
using SteelX.Shared.Game;
using SteelX.Shared.Packets;
//using NetCoreServer;
//using Console = Colorful.Console;

namespace SteelX.Shared
{
	/// <summary>
	/// The game session represents a single connected user
	/// </summary>
	/// The user may or may not be logged in
	//ToDo: Change to Interface?
	public abstract class GameSession //: TcpSession
	{
		/// <summary>
		/// The user logged into this session
		/// </summary>
		public Player User { get; set; }

		/// <summary>
		/// The protocol version of this client
		/// </summary>
		public int Version { get; set; }

		/// <summary>
		/// The time stamp of when the client connected
		/// </summary>
		public DateTime ConnectedStamp { get; set; }

		public int ConnectedMs { get { return (int)(DateTime.UtcNow - ConnectedStamp).TotalMilliseconds; } }
		
		/// <summary>
		/// The room this session is connected to
		/// </summary>
		//public GameInstance GameInstance { get; set; }
		public Guid InstanceId { get; set; }
		
		/// <summary>
		/// Has this session loaded into the game fully?
		/// </summary>
		//TODO: This should probably be somewhere else
		//TODO: Reset this to false after game
		public bool IsGameReady { get; set; }

		/// <summary>
		/// Is the user signed in?
		/// </summary>
		public bool IsSignedIn { get { return User != null; } }

		//public GameSession(TcpServer server) : base(server)
		//{
		//}

		/// <summary>
		/// Gets the username for this session if they are logged in
		/// </summary>
		/// <returns></returns>
		//public string GetUserName()
		//{
		//	return User != null ? User.Username : "[NOT SIGNED IN]";
		//}

		/// <summary>
		/// Send a packet from this client
		/// </summary>
		/// <param name="packet"></param>
		public abstract void SendPacket(ServerBasePacket packet);
		//{
		//	byte[] data = packet.Write();
		//	
		//	SendAsync(data);
		//	
		//	//TODO: Add config here - if debug
		//	//Console.WriteLine("[S] 0x{0:x2} {1} >>> {2}", Color.Green, packet.GetId(), packet.GetType(), GetUserName());
		//}

		protected virtual void OnConnected()
		{
			// Stamp when they connect for calculating ping
			ConnectedStamp = DateTime.UtcNow;
			
			//Console.WriteLine($"[{ConnectedStamp}] Game TCP session with Id {Id} connected!");
		}

		protected abstract void OnDisconnected();
		//{
		//	//Console.WriteLine($"Game TCP session with Id {Id} disconnected!");
		//}

		protected abstract void OnReceived(byte[] buffer, long offset, long size);
		//{
		//	var pos = 0;
		//	while (pos < size)
		//	{
		//		var packet = PacketHandler.HandlePacket(buffer, pos, this);
		//		pos += packet.Size;
		//		
		//		//TODO: Add config here - if debug
		//		//Console.WriteLine("[C] 0x{0:x2} {1} <<< {2}", Color.Red, packet.Id, packet.GetType(), GetUserName());
		//
		//		//TODO: RUN PACKET ASYNC IF LOW PRIORITY?
		//		packet.Run();
		//	}
		//}

		protected void OnError(SocketError error)
		{
			//Console.WriteLine($"Game TCP session caught an error with code {error}");
		}
		
		#region THREAD
		/*private bool _inGame;
		private DateTime _lastTick;

		/// <summary>
		/// Starts the users game thread
		/// </summary>
		public async void StartGameThread()
		{
			// Set running flag
			_inGame = true;
			
			// Set first tick
			_lastTick = DateTime.UtcNow;

			// Main loop
			while (_inGame)
			{
				// Calculate delta
				var delta = (float)(DateTime.UtcNow - _lastTick).TotalMilliseconds;

				// Update
				_lastTick = DateTime.UtcNow;
				
				// Call game session tick
				this.GameInstance.TickUnit(User.CurrentUnit, delta);
				
				// Sleep
				//await System.Threading.Tasks.Task.Delay(50);
				System.Threading.Thread.Sleep(50);
			}
		}*/
		#endregion
	}
}