using System;
using System.Linq;
using SteelX.Shared;
//using System.Drawing;
//using GameServer.ServerPackets.Game;
//using Console = Colorful.Console;

namespace SteelX.Client.Packets.Game
{
	/// <summary>
	/// Sent frequently by the user to indicate their current position
	/// </summary>
	public class MoveUnit : ClientGameBasePacket
	{
		private readonly bool _shouldUpdate;

		public override Shared.PacketTypes PacketType
		{
			get
			{
				return Shared.PacketTypes.GAME_MOVE_UNIT;
			}
		}

		public MoveUnit(byte[] data, GameSession client) : base(data, client)
		{
			/*if (Unit.IgnorePackets > 0)
			{
				Unit.IgnorePackets--;
				_shouldUpdate = false;
				Console.WriteLine("IGNORED MOVE PACKET", Color.Pink);
				return;
			}
			TickUnit();
			//GetInt(); // ClientTime? - Not sure what to do with this yet - ping check? - maybe packet number?

			// Read flags
			Unit.Movement = GetByte();
			Unit.UnknownMovementFlag = GetByte();
			Unit.Boosting = GetByte();
			
			//System.Console.WriteLine("Unit Move {0} Unknown {1} Boost {2}", Unit.Movement, Unit.UnknownMovementFlag, Unit.Boosting);

			// Read the position information
			GetUnitPositionAndAim();

			_shouldUpdate = true;*/
		}

		/*public override string GetType()
		{
			return "GAME_MOVE_UNIT";
		}*/

		protected override void RunImpl()
		{
			//if (_shouldUpdate)
			//	GetClient().GameInstance.UpdateUnitPosition(Unit);
		}
	}
}