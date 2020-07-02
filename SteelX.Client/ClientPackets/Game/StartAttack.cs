using System;
using System.Drawing;
using System.Linq;
using GameServer.ServerPackets.Game;
using Console = Colorful.Console;

namespace GameServer.ClientPackets.Game
{
    /// <summary>
    /// Sent when a user begins attacking
    /// </summary>
    public class StartAttack : ClientGameBasePacket
    {
        /// <summary>
        /// The arm they are attacking with
        /// </summary>
        private readonly int _arm;

        private readonly int _comboStep;
        
        public StartAttack(byte[] data, GameSession client) : base(data, client)
        {
//            // Check practice mode
            if (GetClient().GameInstance == null) return;


            TickUnit();
            //Console.WriteLine("Timestamp?? - : {0}", GetInt()); // ??

            _arm = GetInt();
            _comboStep = GetInt();
            
            // Read the position information
            GetUnitPositionAndAim();
            
            System.Console.WriteLine("Got aim: X: {0:F} Y: {1:F}", Unit.AimX/100.0f, Unit.AimY/100.0f);
        }

        public override string GetType()
        {
            return "START_ATTACK";
        }

        protected override void RunImpl()
        {
            // Check practice mode
            if (GetClient().GameInstance == null) return;

            
            var weapon = Unit.GetWeaponByArm(_arm);

            weapon.ComboStep = _comboStep;
            
            GetClient().GameInstance.TryAttack(Unit, _arm);
            
            if (weapon.IsAutomatic)
            {
                // For machine guns
                weapon.IsAttacking = true;
            }
        }
    }
}