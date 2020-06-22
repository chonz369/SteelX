namespace GameServer.ServerPackets.Inventory
{
    /// <summary>
    /// Sends the ID of the users default unit
    /// Maybe for lobby screen?
    /// </summary>
    public class SendDefaultUnit : ServerBasePacket
    {
        /// <summary>
        /// The id of the default unit
        /// </summary>
        private readonly uint _defaultId;
        
        public SendDefaultUnit(uint defaultId)
        {
            _defaultId = defaultId;
        }
        
        public override string GetType()
        {
            return "INV_SEND_DEFAULT_UNIT";
        }

        public override byte GetId()
        {
            return 0x1d;
        }

        protected override void WriteImpl()
        {
            WriteUInt(_defaultId);
        }
    }
}