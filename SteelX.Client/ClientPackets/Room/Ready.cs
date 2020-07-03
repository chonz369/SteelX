using SteelX.Shared;
//using GameServer.ServerPackets.Room;

namespace SteelX.Client.Packets.Room
{
	/// <summary>
	/// Sent when a user toggles their ready status
	/// </summary>
	public class Ready : ClientBasePacket
	{
		/// <summary>
		/// Are they ready
		/// </summary>
		private readonly bool _ready;

		public override Shared.PacketTypes PacketType
		{
			get
			{
				return Shared.PacketTypes.ROOM_READY;
			}
		}

		public Ready(byte[] data, GameSession client) : base(data, client)
		{
			_ready = GetBool();
		}

		/*public override string GetType()
		{
			return "ROOM_READY";
		}*/

		protected override void RunImpl()
		{
			var client = GetClient();
			client.User.IsReady = _ready;
			
			client.GameInstance.MulticastPacket(new UserInfo(client.GameInstance, client.User));
		}
	}
}