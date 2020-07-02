using Data.Model;

namespace GameServer.ServerPackets.Game
{
    /// <summary>
    /// Looks to update unit status, maybe for HP or death
    /// </summary>
    public class StatusChanged : ServerBasePacket
    {
        private readonly Unit _unit;
        private readonly int _a;
        private readonly int _b;
        private readonly int _c;

        public StatusChanged(Unit unit, int a, int b, int c)
        {
            _unit = unit;

            _a = a;
            _b = b;
            _c = c;
        }
        
        public override string GetType()
        {
            return "STATUS_CHANGED";
        }

        public override byte GetId()
        {
            return 0x7a;
        }

        protected override void WriteImpl()
        {
            WriteUInt(_unit.Id); // UserId
            WriteInt(_a); // Can attack? 0 = no 1 = yes?
            WriteInt(_b); // If zero, sheild effect, if 1, not?
            WriteInt(_c); // Can move? if 0 cant, if 1 can?
        }
    }
}