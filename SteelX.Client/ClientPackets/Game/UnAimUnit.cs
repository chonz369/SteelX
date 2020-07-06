using System;
using System.Linq;
using SteelX.Shared;
//using System.Drawing;
//using Data.Model;
//using Console = Colorful.Console;

namespace SteelX.Client.Packets.Game
{
	/// <summary>
	/// Sent when the user stops aiming at a unit
	/// </summary>
	public class UnAimUnit : ClientGameBasePacket
	{
		private readonly Mechanaught _oldTarget;

		public override Shared.PacketTypes PacketType
		{
			get
			{
				return Shared.PacketTypes.UN_AIM_UNIT;
			}
		}

		public UnAimUnit(byte[] data, GameSession client) : base(data, client)
		{
			// Check practice mode
			/*if (GetClient().GameInstance == null) return;

			TickUnit();
			//GetInt(); // ClientTime? - Not sure what to do with this yet - ping check? - maybe packet number?

			var arm = GetInt();
			
			// Save old target
			_oldTarget = Unit.GetWeaponByArm(arm).Target;
			
			// Update to no target
			var weapon = Unit.GetWeaponByArm(arm);
			
			if (weapon != null)
				weapon.Target = null;
			else
				Console.WriteLine("Cant assign target, 2 handed weapon! Unit {0} Arm {1}", (object)Unit.Id, arm);*/
		}

		/*public override string GetType()
		{
			return "UN_AIM_UNIT";
		}*/

		protected override void RunImpl()
		{
			// Check practice mode
			//if (GetClient().GameInstance == null) return;

			//GetClient().GameInstance.UnAimUnit(Unit, _oldTarget);

			//Sends a request to remove Lock-on from target
			//Client sends an Id of the lock-on (so that packet ordering doesnt conflict)
			//Server should respond back to client repeating (broadcasting) command, to confirm recieved
		}
	}
}