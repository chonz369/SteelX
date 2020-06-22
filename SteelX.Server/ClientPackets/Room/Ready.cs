using GameServer.ServerPackets.Room;

namespace GameServer.ClientPackets.Room
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
        
        public Ready(byte[] data, GameSession client) : base(data, client)
        {
            _ready = GetBool();
        }

        public override string GetType()
        {
            return "ROOM_READY";
        }

        protected override void RunImpl()
        {
            var client = GetClient();
            client.User.IsReady = _ready;
            
            client.GameInstance.MulticastPacket(new UserInfo(client.GameInstance, client.User));
        }
    }
}