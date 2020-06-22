using System;
using System.Drawing;
using System.Linq;
using GameServer.ServerPackets.Game;
using Console = Colorful.Console;

namespace GameServer.ClientPackets.Game
{
    /// <summary>
    /// Sent frequently by the user to indicate their current position
    /// </summary>
    public class MoveUnit : ClientGameBasePacket
    {
        public MoveUnit(byte[] data, GameSession client) : base(data, client)
        {
            TickUnit();
            //GetInt(); // ClientTime? - Not sure what to do with this yet - ping check? - maybe packet number?

            // Read flags
            Unit.Movement = GetByte();
            Unit.UnknownMovementFlag = GetByte();
            Unit.Boosting = GetByte();

            // Read the position information
            GetUnitPositionAndAim();
        }

        public override string GetType()
        {
            return "GAME_MOVE_UNIT";
        }

        protected override void RunImpl()
        {
            GetClient().GameInstance.UpdateUnitPosition(Unit);
        }
    }
}