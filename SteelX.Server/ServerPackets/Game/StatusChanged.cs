namespace GameServer.ServerPackets.Game
{
    /// <summary>
    /// Looks to update unit status, maybe for HP or death
    /// </summary>
    public class StatusChanged : ServerBasePacket
    {
        public override string GetType()
        {
            return "STATUS_CHANGED";
        }

        public override byte GetId()
        {
            return 0x74;
        }

        protected override void WriteImpl()
        {
            WriteInt(1); // UserId
            WriteInt(1); // Can attack? 0 = no 1 = yes?
            WriteInt(1); // If zero, sheild effect, if 1, not?
            WriteInt(1); // Can move? if 0 cant, if 1 can?
        }
    }
}