namespace SteelX.Shared.Packets
{
	/// <summary>
	/// Base packet for all packets in the server
	/// </summary>
	public abstract class BasePacket
	{
		public abstract PacketTypes PacketType { get; }
		/// <summary>
		/// The client session this packet is for
		/// </summary>
		private readonly GameSession _client;

		protected BasePacket()
		{
			
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