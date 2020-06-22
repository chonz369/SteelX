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
        
        private const int damage = 297;
        
        public StartAttack(byte[] data, GameSession client) : base(data, client)
        {
            Console.WriteLine("Packet size: {0}",Color.Coral, Size);
            
            Console.WriteLine("Packet raw: {0}", Color.Coral,
                String.Join(" - ", _raw.Select(b => b.ToString("X2")).ToArray()));

            TickUnit();
            //Console.WriteLine("Timestamp?? - : {0}", GetInt()); // ??

            _arm = GetInt();
            
            Console.WriteLine("Arm?? - : {0}", _arm); // ??
            Console.WriteLine("Int?? - : {0}", GetInt()); // ??
            
            // Read the position information
            GetUnitPositionAndAim();
        }

        public override string GetType()
        {
            return "START_ATTACK";
        }

        protected override void RunImpl()
        {
            GetClient().GameInstance.TryAttack(Unit, _arm);

            var weapon = Unit.GetWeaponByArm(_arm);
            
            if (weapon.IsAutomatic)
            {
                // For machine guns
                weapon.IsAttacking = true;
            }
        }
    }
}