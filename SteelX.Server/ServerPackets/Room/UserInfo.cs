using SteelX.Shared;
using SteelX.Server;
using SteelX.Server.Game;

namespace SteelX.Server.Packets.Room
{
	/// <summary>
	/// Contains the public user info for users in a room
	/// </summary>
	public class UserInfo : ServerBasePacket
	{
		/// <summary>
		/// The user contained in this packet
		/// </summary>
		private readonly Player _user;

		private readonly GameInstance _room;

		public UserInfo(GameInstance room, Player user)
		{
			_room = room;
			_user = user;
		}

		public override Shared.PacketTypes PacketType
		{
			get
			{
				return Shared.PacketTypes.ROOM_USER_INFO;
			}
		}

		/*public override string GetType()
		{
			return "ROOM_USER_INFO";
		}

		public override byte GetId()
		{
			return 0x35;
		}*/

		protected override void WriteImpl()
		{
			this.WriteRoomUserInfo(_room, _user);
		}
	}
}