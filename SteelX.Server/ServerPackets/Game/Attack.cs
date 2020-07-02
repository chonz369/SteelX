using System;
using System.Numerics;
using Data.Model;
using Data.Model.Items;

namespace GameServer.ServerPackets.Game
{
    /// <summary>
    /// Testing this - 69 (nice) seems to be single target attacks
    /// </summary>
    public class Attack : ServerBasePacket
    {
        private readonly Unit _unit;
        private readonly int _arm;
        private readonly Weapon _weapon;

        public Attack(Unit unit, int arm, Weapon weapon)
        {
            _unit = unit;
            _arm = arm;
            _weapon = weapon;
        }
        
        public override string GetType()
        {
            return "ATTACK";
        }

        public override byte GetId()
        {
            return 0x67;
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

            if (_weapon.RecoilDistance > 0)
            {
                var direction = new Vector3((float)Math.Cos(_unit.AimX), (float)Math.Sin(_unit.AimX), 0);
                direction *= _weapon.RecoilDistance;

                _unit.WorldPosition += direction;
            }
            
            WriteShort(_unit.AimY); // Attacker - AimX
            WriteShort(_unit.AimX); // Attacker - AimY
            
            WriteFloat(_unit.WorldPosition.X); // Attacker - X
            WriteFloat(_unit.WorldPosition.Y); // Attacker - Y
            WriteFloat(_unit.WorldPosition.Z); // Attacker - Z
            
            WriteFloat(_weapon.CurrentOverheat); // Overheat, yess
        }
    }
}