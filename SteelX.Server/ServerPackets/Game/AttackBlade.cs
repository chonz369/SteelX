using Data.Model;
using Data.Model.Items;

namespace GameServer.ServerPackets.Game
{
    /// <summary>
    /// Sent when a user performs a melee attack
    /// </summary>
    public class AttackBlade : ServerBasePacket
    {
        private readonly Unit _unit;
        private readonly int _arm;
        private readonly Weapon _weapon;
        private readonly int _serverTime;
        
        public static int COMBOSTEP = 0;
        public static int AIR = 0;
        public static int PUSH = 0;
        public static int RESULT = 0;

        public AttackBlade(Unit unit, int arm, Weapon weapon, int serverTime)
        {
            _unit = unit;
            _arm = arm;
            _weapon = weapon;
            _serverTime = serverTime;
        }
        
        public override string GetType()
        {
            return "ATTACK_BLADE";
        }

        public override byte GetId()
        {
            return 0x6d;
        }

        protected override void WriteImpl()
        {
            var targetId = _weapon.Target?.Id ?? 0xFFFFFFFF;
            
            WriteInt(_serverTime); // Unknown
            WriteInt(targetId == -1 ? 2 : 1); // Result code
            WriteUInt(_unit.Id); // Attacker Id - maybe passed as an index?
            WriteInt(_arm); // Arm?
            
            // Maybe grouped?
            WriteUInt(targetId); // Victim id?
            WriteInt(_weapon.Damage); // Damage
            
            // All ye who enter here beware - no mans land!
            WriteByte((byte)PUSH); // Should push back?
            
            // BLOCK - 1 VEC, 2 INTS AND A BYTE
            WriteFloat(0); // Offset vector for pushback
            WriteFloat(0); // Offset vector for pushback
            WriteFloat(300.0f); // Offset vector for pushback
            
            WriteInt(0); // Other target Ids
            WriteInt(0); // Other damage
            
            WriteByte(0);
            // BLOCK END
            
            // BLOCK - 1 VEC, 2 INTS AND A BYTE
            WriteFloat(0); // Unknown
            WriteFloat(0); // Unknown
            WriteFloat(0); // Unknown
            
            WriteInt(0);
            WriteInt(0);
            
            WriteByte(0);
            // BLOCK END
            
            // BLOCK - 1 VEC, 2 INTS AND A BYTE
            WriteFloat(0); // Unknown
            WriteFloat(0); // Unknown
            WriteFloat(0); // Unknown
            
            WriteInt(0);
            WriteInt(0);
            
            WriteByte(0);
            // BLOCK END
            
            // Ends with one last Vec and Int
            WriteFloat(0); // Unknown
            WriteFloat(0); // Unknown
            WriteFloat(0); // Unknown
            
            WriteInt(_weapon.ComboStep); // COMBO STEP
            
            // Standard
            // Test
            if (RESULT == 1)
                _unit.AimY = -3000;
            else
                _unit.AimY = 0;
            
            WriteShort(_unit.AimY); // Attacker - AimX

            // Temp
            _unit.AimX = 0;
            
            WriteShort(_unit.AimX); // Attacker - AimY

            if (RESULT == 1)
            {
                _unit.WorldPosition.X += 115.9111f;
                _unit.WorldPosition.Z -= 31.05829f;
            }
            else
                _unit.WorldPosition.X += 120.0f;
//            _unit.WorldPosition.Y += 100.0f;
//            _unit.WorldPosition.Z += (AIR == 1 ? -50.0f : 0.0f);
            
            WriteFloat(_unit.WorldPosition.X); // Attacker - X
            WriteFloat(_unit.WorldPosition.Y); // Attacker - Y
            WriteFloat(_unit.WorldPosition.Z); // Attacker - Z
            
            WriteByte((byte)AIR); // Unknown - air related?
            
            WriteFloat(_weapon.CurrentOverheat); // Overheat, yess
        }
    }
}