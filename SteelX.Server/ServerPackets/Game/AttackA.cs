using System;
using SteelX.Shared;
using SteelX.Server;
//using Data.Model;
//using Data.Model.Items;

namespace SteelX.Server.Packets.Game
{
	/// <summary>
	/// Testing this - 69 (nice) seems to be single target attacks
	/// </summary>
	public class AttackA : ServerBasePacket
	{
		private readonly Mechanaught _unit;
		private readonly int _arm;
		private readonly Weapon _weapon;

		public override Shared.PacketTypes PacketType
		{
			get
			{
				return Shared.PacketTypes.ATTACK_A;
			}
		}

		public AttackA(Mechanaught unit, int arm, Weapon weapon)
		{
			_unit = unit;
			_arm = arm;
			_weapon = weapon;
		}
		
		/*public override string GetType()
		{
			return "ATTACK_A";
		}

		public override byte GetId()
		{
			return 0x6f;
		}*/

		protected override void WriteImpl()
		{
			//uint targetId = _weapon.Target?.Id ?? 0;
			//Console.WriteLine("IFO TYPE: {0}", _weapon.IfoType);

			WriteInt(0); // Time?
			WriteInt(1); // Result code - For hit, crit, sheild
			WriteUInt(_unit.Id); // Attacker Id - maybe passed as an index?
			WriteInt(_arm); // Arm?
			WriteUInt(_weapon.Id); // IFO Id
			WriteInt(0); // Unknown - damage? Id?
			
			WriteShort(_unit.AimY); // Attacker - AimX
			WriteShort(_unit.AimX); // Attacker - AimY
			
			WriteFloat(_unit.WorldPosition.X); // Attacker - X
			WriteFloat(_unit.WorldPosition.Y); // Attacker - Y
			WriteFloat(_unit.WorldPosition.Z); // Attacker - Z
			
			WriteFloat(_weapon.CurrentOverheat); // Overheat, yess
		}
	}
}