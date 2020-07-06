using SteelX.Shared;

namespace SteelX.Client.Packets.Game
{
	/// <summary>
	/// Sent when the user stops attacking with an automatic weapon
	/// </summary>
	public class StopAttack : ClientGameBasePacket
	{
		/// <summary>
		/// The arm they are using
		/// </summary>
		private readonly int _arm;

		public override Shared.PacketTypes PacketType
		{
			get
			{
				return Shared.PacketTypes.STOP_ATTACK;
			}
		}

		public StopAttack(byte[] data, GameSession client) : base(data, client)
		{
			_arm = GetInt();
			
			// Standard position update
			GetUnitPositionAndAim();
		}

		/*public override string GetType()
		{
			return "STOP_ATTACK";
		}*/

		protected override void RunImpl()
		{
			var weapon = Unit.GetWeaponByArm(_arm);

			// For machine guns
			//weapon.IsAttacking = false;

			//Sends a ping to server that client has released mouse/attack button
			//Client should identify which buton was released (MouseUp/KeyboardUp)
			//Server should respond back to client repeating (broadcasting) command, to confirm recieved
		}
	}
}