using System;
using System.Drawing;
using System.Linq;
using Data.Model;
using Console = Colorful.Console;

namespace GameServer.ClientPackets.Game
{
    /// <summary>
    /// Sent when the user stops aiming at a unit
    /// </summary>
    public class UnAimUnit : ClientGameBasePacket
    {
        private readonly Unit _oldTarget;
        
        public UnAimUnit(byte[] data, GameSession client) : base(data, client)
        {
            TickUnit();
            //GetInt(); // ClientTime? - Not sure what to do with this yet - ping check? - maybe packet number?

            var arm = GetInt();
            
            // Save old target
            _oldTarget = Unit.GetWeaponByArm(arm).Target;
            
            // Update to no target
            Unit.GetWeaponByArm(arm).Target = null;
        }

        public override string GetType()
        {
            return "UN_AIM_UNIT";
        }

        protected override void RunImpl()
        {
            GetClient().GameInstance.UnAimUnit(Unit, _oldTarget);
        }
    }
}