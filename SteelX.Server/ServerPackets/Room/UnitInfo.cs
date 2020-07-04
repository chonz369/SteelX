using SteelX.Shared;
using SteelX.Server;

namespace SteelX.Server.Packets.Room
{
	/// <summary>
	/// Contains unit info for units in a room
	/// </summary>
	public class UnitInfo : ServerBasePacket
	{
		/// <summary>
		/// The user who owns this unit
		/// </summary>
		private readonly Player _user;
		
		/// <summary>
		/// The unit
		/// </summary>
		private readonly Mechanaught _unit;

		public UnitInfo(Player user, Mechanaught unit)
		{
			_unit = unit;
			_user = user;
		}

		public override Shared.PacketTypes PacketType
		{
			get
			{
				return Shared.PacketTypes.ROOM_UNIT_INFO;
			}
		}

		/*public override string GetType()
		{
			return "ROOM_UNIT_INFO";
		}

		public override byte GetId()
		{
			return 0x3e;
		}*/

		protected override void WriteImpl()
		{
			WriteUInt(_user.Id);
			WriteUInt(_unit.Id);
			WriteUInt(_user.Team);
			
			WriteString(_unit.Name);
			
			this.WritePartInfo(_unit.Head);
			this.WritePartInfo(_unit.Chest);
			this.WritePartInfo(_unit.Arms);
			this.WritePartInfo(_unit.Legs);
			this.WritePartInfo(_unit.Backpack);
			
			this.WritePartInfo(_unit.WeaponSet1Left);
			this.WritePartInfo(_unit.WeaponSet1Right);
			
			this.WritePartInfo(_unit.WeaponSet2Left);
			this.WritePartInfo(_unit.WeaponSet2Right);
			
			WriteInt(0); // Unknown - skills count?
		}
	}
}