using SteelX.Shared;
using SteelX.Server;
//using Data.Model;

namespace SteelX.Server.Packets.Game
{
	/// <summary>
	/// Sent when a user switches weapon sets
	/// </summary>
	public class WeaponsetChanged : ServerBasePacket
	{
		private readonly Mechanaught _unit;

		public override Shared.PacketTypes PacketType
		{
			get
			{
				return Shared.PacketTypes.WEAPONSET_CHANGED;
			}
		}

		public WeaponsetChanged(Mechanaught unit)
		{
			_unit = unit;
		}
		
		/*public override string GetType()
		{
			return "WEAPONSET_CHANGED";
		}

		public override byte GetId()
		{
			return 0x84;
		}*/

		protected override void WriteImpl()
		{
			WriteUInt(_unit.Id); // Unit Id
			WriteInt(_unit.CurrentWeaponSet); // Slot
			WriteFloat(_unit.GetWeaponByArm(0).CurrentOverheat); // Current overheat
			WriteFloat(_unit.GetWeaponByArm(1) != null ? _unit.GetWeaponByArm(1).CurrentOverheat : 0); // Current overheat
			
			WriteBool(_unit.GetWeaponByArm(0).IsOverheated); // Is overheated
			WriteBool(_unit.GetWeaponByArm(1) != null ? _unit.GetWeaponByArm(1).IsOverheated : false); // Is overheated
		}
	}
}