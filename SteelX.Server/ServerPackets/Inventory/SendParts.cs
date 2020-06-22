using Data.Model;

namespace GameServer.ServerPackets.Inventory
{
    /// <summary>
    /// Sends all the parts (and other things?) in the users inventory
    /// </summary>
    public class SendParts : ServerInventoryBasePacket
    {
        public SendParts(ExteelUser user) : base(user)
        {
        }

        public override string GetType()
        {
            return "INV_SEND_PARTS";
        }

        public override byte GetId()
        {
            return 0x1f;
        }

        protected override void WriteImpl()
        {
            WriteInt(Inventory.Parts.Count);

            foreach (var part in Inventory.Parts)
            {
                this.WritePartInfo(part);
            }
        }
    }
}