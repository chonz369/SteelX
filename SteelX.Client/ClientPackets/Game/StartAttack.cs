using System;
using System.Linq;
using SteelX.Shared;
//using System.Drawing;
//using GameServer.ServerPackets.Game;
//using Console = Colorful.Console;

namespace SteelX.Client.Packets.Game
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

		public override Shared.PacketTypes PacketType
		{
			get
			{
				return Shared.PacketTypes.START_ATTACK;
			}
		}

		public StartAttack(byte[] data, GameSession client) : base(data, client)
		{
			// Check practice mode
			//if (GetClient().GameInstance == null) return;


			TickUnit();
			//Console.WriteLine("Timestamp?? - : {0}", GetInt()); // ??

			_arm = GetInt();
			_comboStep = GetInt();
			
			// Read the position information
			GetUnitPositionAndAim();
			
			System.Console.WriteLine("Got aim: X: {0:F} Y: {1:F}", Unit.AimX/100.0f, Unit.AimY/100.0f);
		}

		/*public override string GetType()
		{
			return "START_ATTACK";
		}*/

		protected override void RunImpl()
		{
			// Check practice mode
			/*if (GetClient().GameInstance == null) return;

			
			var weapon = Unit.GetWeaponByArm(_arm);

			weapon.ComboStep = _comboStep;
			
			GetClient().GameInstance.TryAttack(Unit, _arm);
			
			if (weapon.IsAutomatic)
			{
				// For machine guns
				weapon.IsAttacking = true;
			}*/

			//Sends a ping to server on ButtonDown
			//If weapon is single fire, it fires a single shot
			//if weapon is automatic fire, the trigger is held down
			//if weapon is melee, the combo is continued, if done in succession
			//Server should respond back to client repeating (broadcasting) command, to confirm recieved
		}
	}
}