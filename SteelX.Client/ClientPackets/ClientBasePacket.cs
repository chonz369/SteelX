using System;
using System.Text;
using SteelX.Shared;
using SteelX.Shared.Packets;
//using GameServer.ServerPackets;
//using Console = Colorful.Console;

namespace SteelX.Client.Packets
{
	/// <summary>
	/// Base class for all packets received from client
	/// </summary>
	//public abstract class ClientBasePacket : BasePacket
	public abstract class ClientBasePacket : SteelX.Shared.Packets.ClientBasePacket
	{
		/// <summary>
		/// Position within the packet we have read to
		/// </summary>
		private int _index;
		
		protected ClientBasePacket(byte[] data, GameSession client) : 
			base(data, client) //base(client)
		{
			//_raw = data;
			//
			//// Skip header
			//_index = 3;
		}

		#region Methods
		/// <summary>
		/// Gets a string from the buffer, specified by a size
		/// If we ever encounter fixed size strings, this will need to be made protected
		/// </summary>
		/// <param name="size"></param>
		/// <returns></returns>
		private string GetString(int size)
		{
			byte[] newArray = new byte[size];
			Array.Copy(_raw, _index, newArray, 0, size);
			_index += size;
			return Encoding.Unicode.GetString(newArray);
		}        
		#endregion
	}
}