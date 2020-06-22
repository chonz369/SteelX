using Data.Model;

namespace GameServer.ServerPackets.Game
{
    /// <summary>
    /// Sent when the user sucessfully or unsucessfully respawns i think
    /// </summary>
    public class RegainResult : ServerBasePacket
    {
        private readonly Unit _unit;

        public RegainResult(Unit unit)
        {
            _unit = unit;
        }
        
        public override string GetType()
        {
            return "GAME_REGAIN_RESULT";
        }

        public override byte GetId()
        {
            return 0x52;
        }

        protected override void WriteImpl()
        {
            WriteInt(0); // Result code?
            WriteUInt(_unit.Id); // Unit id?
        }
    }
}