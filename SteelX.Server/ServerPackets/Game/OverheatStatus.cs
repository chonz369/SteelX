using SteelX.Shared;
using SteelX.Server;
//using SteelX.Server.Items;

namespace SteelX.Server.Packets.Game
{
	/// <summary>
	/// Looks to include data about the overheat status for a users weapon
	/// </summary>
	public class OverheatStatus : ServerBasePacket
	{
		private readonly Weapon _weapon;
		private readonly int _arm;
		private readonly int _slot;
		private readonly Mechanaught _unit;

		public OverheatStatus(Mechanaught unit, int arm, int slot, Weapon weapon)
		{
			_unit = unit;
			_arm = arm;
			_slot = slot;
			_weapon = weapon;
		}

		public override Shared.PacketTypes PacketType
		{
			get
			{
				return Shared.PacketTypes.OVERHEAT_STATUS;
			}
		}

		/*public override string GetType()
		{
			return "OVERHEAT_STATUS";
		}

		public override byte GetId()
		{
			return 0x83;
		}*/

		protected override void WriteImpl()
		{
			WriteUInt(_unit.Id); // UnitId
			WriteFloat(_weapon.CurrentOverheat); // Overheat value - seems to be absolute...
			//TODO: Should slow and arm be stored on weapon?
			WriteInt(_slot); // SlotNumber
			WriteByte((byte)_arm); // Hand
			WriteBool(_weapon.IsOverheated); // Status
		}
	}
}