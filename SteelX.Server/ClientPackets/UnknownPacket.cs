using System;

namespace GameServer.ClientPackets
{
    /// <summary>
    /// Dummy packet for unknown client packets
    /// </summary>
    public class UnknownPacket : ClientBasePacket
    {
        public UnknownPacket(byte[] data, GameSession client) : base(data, client)
        {
        }

        public override string GetType()
        {
            return "UNKNOWN";
        }

        protected override void RunImpl()
        {
            // Do nothing
        }
    }
}