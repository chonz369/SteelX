using SteelX.Shared;
using SteelX.Server;

namespace SteelX.Server.Packets.Game
{
	/// <summary>
	/// Sent when someone DIES
	/// </summary>
	public class UnitDestroyed : ServerBasePacket
	{
		private readonly Mechanaught _killer;
		private readonly Mechanaught _victim;

		public UnitDestroyed(Mechanaught victim, Mechanaught killer)
		{
			_killer = killer;
			_victim = victim;
		}

		public override Shared.PacketTypes PacketType
		{
			get
			{
				return Shared.PacketTypes.UNIT_DESTROYED;
			}
		}

		/*public override string GetType()
		{
			return "UNIT_DESTROYED";
		}

		public override byte GetId()
		{
			return 0x62;
		}*/

		protected override void WriteImpl()
		{
			var killerId = _killer?.Id ?? 0;
			
			WriteInt(0); // Unknown
			WriteUInt(killerId); // Killer
			WriteUInt(_victim.Id); // VictimId
			WriteInt(1); // Unknown
		}
	}
}