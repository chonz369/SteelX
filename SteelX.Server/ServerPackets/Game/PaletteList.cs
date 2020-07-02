using Data.Model;

namespace GameServer.ServerPackets.Game
{
    /// <summary>
    /// Seems to be sent with a list of "palette"
    /// Maybe skills?
    /// </summary>
    public class PaletteList : ServerBasePacket
    {
        /// <summary>
        /// The unit whos data to send
        /// </summary>
        private readonly Unit _unit;

        public PaletteList(Unit unit)
        {
            _unit = unit;
        }
        
        public override string GetType()
        {
            return "GAME_PALETTE_LIST";
        }

        public override byte GetId()
        {
            return 0x4b;
        }

        protected override void WriteImpl()
        {
            WriteUInt(_unit.Id);
            
            WriteInt(0); // Unknown
            WriteInt(0); // Unknown
        }
    }
}