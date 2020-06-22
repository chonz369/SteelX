using Data.Model;

namespace GameServer.ServerPackets.Inventory
{
    /// <summary>
    /// This looks like a packet for skills in the users inventory
    /// </summary>
    public class SendCodeList : ServerInventoryBasePacket
    {
        public SendCodeList(ExteelUser user) : base(user)
        {
        }

        public override string GetType()
        {
            return "INV_CODE_LIST";
        }

        public override byte GetId()
        {
            return 0x1e;
        }

        protected override void WriteImpl()
        {
            // All unknown
            WriteInt(0); // Unknown
            WriteInt(0); // Unknown
        }
    }
}