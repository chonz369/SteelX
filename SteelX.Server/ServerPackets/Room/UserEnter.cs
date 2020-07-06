using SteelX.Shared;
using SteelX.Server;
using SteelX.Server.Game;

namespace SteelX.Server.Packets.Room
{
	/// <summary>
	/// Sent to players in a room when a user joins
	/// </summary>
	public class UserEnter : ServerBasePacket
	{
		/// <summary>
		/// The user contained in this packet
		/// </summary>
		private readonly Player _user;

		private readonly GameInstance _room;

		public UserEnter(GameInstance room, Player user)
		{
			_room = room;
			_user = user;
		}

		public override Shared.PacketTypes PacketType
		{
			get
			{
				return Shared.PacketTypes.ROOM_USER_ENTER;
			}
		}

		/*public override string GetType()
		{
			return "ROOM_USER_ENTER";
		}

		public override byte GetId()
		{
			return 0x0a;
		}*/

		protected override void WriteImpl()
		{
			this.WriteRoomUserInfo(_room, _user);
		}
	}
}