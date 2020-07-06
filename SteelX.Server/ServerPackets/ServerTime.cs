using SteelX.Shared;

namespace SteelX.Server.Packets
{
	/// <summary>
	/// Ping info for the client
	/// </summary>
	public class ServerTime : ServerBasePacket
	{
		private readonly int _clientTime;
		public ServerTime(GameSession client, int clientTime) : base(client)
		{
			_clientTime = clientTime;
		}

		public override Shared.PacketTypes PacketType
		{
			get
			{
				return Shared.PacketTypes.SERVERTIME;
			}
		}
		
		/*public override string GetType()
		{
			return "SERVERTIME";
		}

		public override byte GetId()
		{
			return 0x04;
		}*/

		protected override void WriteImpl()
		{
			// Write the clients time
			//var server = (GameServer)GetClient().Server;
			//WriteInt(server.RunningMs);
			WriteInt(_clientTime);            

			// Write the servers connection time for the client
			WriteInt(GetClient().ConnectedMs);
		}
	}
}