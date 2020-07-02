using System;
using System.Drawing;
using System.Linq;
using Data.Model;
using Console = Colorful.Console;

namespace GameServer.ClientPackets.Game
{
    /// <summary>
    /// Sent when a user aims on a unit
    /// Used to give them a "locked" notification i believe
    /// </summary>
    public class AimUnit : ClientGameBasePacket
    {
        // TODO: Maybe swap this to a lookup of the victim unit?
        // TODO: Maybe a units dictionary in the room?
        private readonly Unit _target;
        private readonly int _arm;
        
        public AimUnit(byte[] data, GameSession client) : base(data, client)
        {
            Console.WriteLine("Packet size: {0}",Color.Coral, Size);
            
            Console.WriteLine("Packet raw: {0}", Color.Coral,
                String.Join(" - ", _raw.Select(b => b.ToString("X2")).ToArray()));
            
            // TODO: This is just for practice. Improve it?
            if (client.GameInstance == null) return;
            TickUnit();
//            GetInt(); // ClientTime? - Not sure what to do with this yet - ping check? - maybe packet number?

            var victimId = GetUInt();

            // Find the target unit
            _target = client.GameInstance.GetUnitById(victimId);
            
            // Find which arm is being used
            _arm = GetInt();
            
            // Read the units position and aim
            GetUnitPositionAndAim();
            
            // Assign target
            var weapon = Unit.GetWeaponByArm(_arm);
            
            if (weapon != null)
                weapon.Target = _target;
            else
                Console.WriteLine("Cant assign target, 2 handed weapon! Unit {0} Arm {1}", Unit.Id, _arm);
        }

        public override string GetType()
        {
            return "AIM_UNIT";
        }

        protected override void RunImpl()
        {
            // Check practice mode
            if (GetClient().GameInstance == null) return;
            
            GetClient().GameInstance.AimUnit(Unit, _target);
        }
    }
}