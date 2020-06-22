using Data.Model;
using Data.Model.Items;

namespace GameServer.ServerPackets.Game
{
    /// <summary>
    /// Testing this - 69 (nice) seems to be single target attacks
    /// </summary>
    public class AttackStart : ServerBasePacket
    {
        private readonly Unit _unit;
        private readonly int _arm;
        private readonly Weapon _weapon;

        public static int RESULT = 0;

        public AttackStart(Unit unit, int arm, Weapon weapon)
        {
            _unit = unit;
            _arm = arm;
            _weapon = weapon;
        }
        
        public override string GetType()
        {
            return "START_ATTACK";
        }

        public override byte GetId()
        {
            return 0x62;
        }

        protected override void WriteImpl()
        {
            var targetId = _weapon.Target?.Id ?? 0;
            
            WriteInt(0); // Unknown
            // TODO: Result codes for overheat?
            WriteInt(1); // Result code
            WriteUInt(_unit.Id); // Attacker Id - maybe passed as an index?
            WriteInt(_arm); // Arm?
            WriteUInt(targetId); // Victim id?
            WriteInt(_weapon.Damage * _weapon.NumberOfShots); // Damage
            
            // If packet type of 0x62 -
            WriteUInt(0); // Unknown
            WriteInt(0); // Unknown
            
            // If packet type of 0x69
//            WriteUInt(targetId); // MultiAttack?
//            WriteInt(_weapon.Damage); // MultiAttack?
//            WriteUInt(targetId); // MultiAttack?
//            WriteInt(_weapon.Damage); // MultiAttack?
//            WriteUInt(targetId); // MultiAttack?
//            WriteInt(_weapon.Damage); // MultiAttack?
            
//            WriteUInt(_unit.Id); // Unknown
//            WriteInt(10); // Unknown
//            WriteUInt(_unit.Id); // Unknown
//            WriteInt(10); // Unknown
            
            WriteShort(_unit.AimX); // Attacker - AimX
            WriteShort(_unit.AimY); // Attacker - AimY
            
            WriteFloat(_unit.WorldPosition.X); // Attacker - X
            WriteFloat(_unit.WorldPosition.Y); // Attacker - Y
            WriteFloat(_unit.WorldPosition.Z); // Attacker - Z
            
            WriteFloat(_weapon.CurrentOverheat); // Overheat, yess
        }
    }
}