using SteelX.Shared;
using SteelX.Server;
using SteelX.Server.Game;

namespace SteelX.Server.Packets.Game
{
	/// <summary>
	/// Sent to all clients in a room to give a users info
	/// </summary>
	public class UserInfo : ServerBasePacket
	{
		/// <summary>
		/// The user this is for
		/// </summary>
		private readonly Player _user;

		private readonly GameInstance _room;

		public UserInfo(GameInstance room, Player user)
		{
			_user = user;
			_room = room;
		}

		public override Shared.PacketTypes PacketType
		{
			get
			{
				return Shared.PacketTypes.GAME_USER_INFO;
			}
		}

		/*public override string GetType()
		{
			return "GAME_USER_INFO";
		}

		public override byte GetId()
		{
			return 0x49;
		}*/

		protected override void WriteImpl()
		{
			this.WriteRoomUserInfo(_room, _user);
		}
	}
}