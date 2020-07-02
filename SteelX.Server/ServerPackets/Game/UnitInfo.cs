using Data.Model;

namespace GameServer.ServerPackets.Game
{
    /// <summary>
    /// Sent in game with data about users units
    /// </summary>
    public class UnitInfo : ServerBasePacket
    {
        private readonly ExteelUser _user;
        private readonly Unit _unit;

        public UnitInfo(ExteelUser user, Unit unit)
        {
            _user = user;
            _unit = unit;
        }
        
        public override string GetType()
        {
            return "GAME_UNIT_INFO";
        }

        public override byte GetId()
        {
            return 0x4c;
        }

        protected override void WriteImpl()
        {
            WriteUInt(_user.Id);
            WriteUInt(_unit.Id);
            
            WriteUInt(_user.Team); // Team
            WriteInt(_unit.Health); // Unit HP?
            
            WriteString(_unit.Name);
            
            this.WritePartInfo(_unit.Head);
            this.WritePartInfo(_unit.Chest);
            this.WritePartInfo(_unit.Arms);
            this.WritePartInfo(_unit.Legs);
            this.WritePartInfo(_unit.Backpack);
            
            this.WritePartInfo(_unit.WeaponSet1Left);
            this.WritePartInfo(_unit.WeaponSet1Right);
            
            this.WritePartInfo(_unit.WeaponSet2Left);
            this.WritePartInfo(_unit.WeaponSet2Right);
        }
    }
}