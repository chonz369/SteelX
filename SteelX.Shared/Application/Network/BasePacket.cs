using System.Collections.Generic;

namespace SteelX.Shared.Packets
{
	/// <summary>
	/// Base packet for all packets in the server
	/// </summary>
	public abstract class BasePacket
	{
		public const int INT_SIZE = 4;
		public const int LONG_SIZE = 8;
		public const int SHORT_SIZE = 2;
		public const int FLOAT_SIZE = 4;
		public const int BOOL_SIZE = 1;

		//public abstract int PacketSize { get; }
		public abstract PacketTypes PacketType { get; }
		//protected Queue<byte> ByteArray { get; private set; }
		protected List<byte> ByteArray { get; private set; }
		/// <summary>
		/// The client session this packet is for
		/// </summary>
		private readonly GameSession _client;

		protected BasePacket()
		{
			ByteArray = new List<byte>();
		}
		
		protected BasePacket(GameSession client)
		{
			_client = client;
		}

		public GameSession GetClient()
		{
			return _client;
		}

		/// <summary>
		/// Gets the packet type of this packet
		/// </summary>
		/// <returns>type of packet</returns>
		new public virtual string GetType() 
		{
			return PacketType.ToString();
		}
	}
}