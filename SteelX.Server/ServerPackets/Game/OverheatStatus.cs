using Data.Model;
using Data.Model.Items;

namespace GameServer.ServerPackets.Game
{
    /// <summary>
    /// Looks to include data about the overheat status for a users weapon
    /// </summary>
    public class OverheatStatus : ServerBasePacket
    {
        private readonly Weapon _weapon;
        private readonly int _arm;
        private readonly Unit _unit;

        public OverheatStatus(Unit unit, int arm, Weapon weapon)
        {
            _unit = unit;
            _arm = arm;
            _weapon = weapon;
        }
        
        public override string GetType()
        {
            return "OVERHEAT_STATUS";
        }

        public override byte GetId()
        {
            return 0x7d;
        }

        protected override void WriteImpl()
        {
            WriteUInt(_unit.Id); // UnitId
            WriteFloat(_weapon.CurrentOverheat); // Overheat value - seems to be absolute...
            // TODO: Should slow and arm be stored on weapon?
            WriteInt(_unit.CurrentWeaponSet); // SlotNumber
            WriteByte((byte)_arm); // Hand
            WriteBool(_weapon.IsOverheated); // Status
        }
    }
}