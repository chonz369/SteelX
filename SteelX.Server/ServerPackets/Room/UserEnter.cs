using Data.Model;
using GameServer.Game;

namespace GameServer.ServerPackets.Room
{
    /// <summary>
    /// Sent to players in a room when a user joins
    /// </summary>
    public class UserEnter : ServerBasePacket
    {
        /// <summary>
        /// The user contained in this packet
        /// </summary>
        private readonly ExteelUser _user;

        private readonly GameInstance _room;

        public UserEnter(GameInstance room, ExteelUser user)
        {
            _room = room;
            _user = user;
        }
        
        public override string GetType()
        {
            return "ROOM_USER_ENTER";
        }

        public override byte GetId()
        {
            return 0x09;
        }

        protected override void WriteImpl()
        {
            this.WriteRoomUserInfo(_room, _user);
        }
    }
}