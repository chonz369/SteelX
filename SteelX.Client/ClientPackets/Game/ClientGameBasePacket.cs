using System;
using SteelX.Shared;
//using Data.Model;
//using GameServer.ServerPackets.Game;
//using Console = Colorful.Console;

namespace SteelX.Client.Packets.Game
{
	/// <summary>
	/// Base packet for game packets from the client
	/// </summary>
	public abstract class ClientGameBasePacket : SteelX.Client.Packets.ClientBasePacket
	{
		/// <summary>
		/// The unit for this client
		/// </summary>
		protected readonly Mechanaught Unit;
		
		protected ClientGameBasePacket(byte[] data, GameSession client) 
			: base(data, client)
		{
			//Unit = GetClient().User.CurrentUnit;
		}

		/// <summary>
		/// Ticks the unit by reading the current time stamp
		/// </summary>
		//ToDo: This is the packet's sequence id, for packet ordering
		//Change method function from void to int, and use for packet conflicts
		protected void TickUnit()
		{
			// Just for practice mode right now
			if (Unit == null) return;
			
			var ping = GetUInt();
			
			var delta = Unit.UpdatePing(ping);
			
			//GetClient().GameInstance?.TickUnit(Unit, delta);
		}

		/// <summary>
		/// Reads the units current aim and pos values from the packet
		/// </summary>
		/// Usually located at the end of the packet
		protected void GetUnitPositionAndAim()
		{
			//TODO: Make this better
			// Just for practice mode right now
			/*if (Unit == null) return;
			
			// Read aim
			Unit.AimY = GetShort();
			Unit.AimX = GetShort();

			//Console.WriteLine("User Aim X: {1:F} Aim Y: {0:F}", Unit.AimY, Unit.AimX);

			// Read position
			Unit.WorldPosition.X = GetFloat();
			Unit.WorldPosition.Y = GetFloat();
			Unit.WorldPosition.Z = GetFloat();*/
		}
	}
}