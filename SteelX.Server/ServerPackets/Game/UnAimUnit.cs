using SteelX.Shared;
using SteelX.Server;

namespace SteelX.Server.Packets.Game
{
	/// <summary>
	/// I think this is the aim unit packet, but it has a lot of data so i could be wrong
	/// </summary>
	public class UnAimUnit : ServerBasePacket
	{
		private readonly Mechanaught _attacker;
		private readonly uint _victim;
		
		public UnAimUnit(Mechanaught attacker, uint victim)
		{
			_attacker = attacker;
			_victim = victim;
		}

		public override Shared.PacketTypes PacketType
		{
			get
			{
				return Shared.PacketTypes.UN_AIM_UNIT;
			}
		}

		/*public override string GetType()
		{
			return "UN_AIM_UNIT";
		}

		public override byte GetId()
		{
			return 0x66;
		}*/

		protected override void WriteImpl()
		{
			WriteInt(0); // Unknown
			WriteUInt(_attacker.Id); // UnitID Attacker
			WriteUInt(_victim); // UnitId Victim - zero means no lock?
			WriteInt(0); // Unknown
			WriteInt(0); // Unknown
		}
	}
}