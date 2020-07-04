using SteelX.Shared;
using SteelX.Server;

namespace SteelX.Server.Packets.Game
{
	/// <summary>
	/// Sent in game with data about users units
	/// </summary>
	public class UnitInfo : ServerBasePacket
	{
		private readonly Player _user;
		private readonly Mechanaught _unit;

		public UnitInfo(Player user, Mechanaught unit)
		{
			_user = user;
			_unit = unit;
		}

		public override Shared.PacketTypes PacketType
		{
			get
			{
				return Shared.PacketTypes.GAME_UNIT_INFO;
			}
		}

		/*public override string GetType()
		{
			return "GAME_UNIT_INFO";
		}

		public override byte GetId()
		{
			return 0x4c;
		}*/

		protected override void WriteImpl()
		{
			WriteUInt(_user.Id);
			WriteUInt(_unit.Id);
			
			WriteUInt(_user.Team); // Team
			WriteInt(_unit.Health); // Unit HP?
			
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
		}
	}
}