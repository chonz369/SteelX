using SteelX.Shared;
using SteelX.Server;

namespace SteelX.Server.Packets.Bridge.Stats
{   
	/// <summary>
	/// A packet containing stats for team battle (territory control)
	/// </summary>
	public class TeamBattleInfo : ServerBasePacket
	{
		private readonly UserStats _stats;
		
		public TeamBattleInfo(UserStats stats)
		{
			_stats = stats;
		}

		public override Shared.PacketTypes PacketType
		{
			get
			{
				return Shared.PacketTypes.BRIDGE_SEND_TEAMBATTLE_INFO;
			}
		}

		/*public override string GetType()
		{
			return $"BRIDGE_SEND_TEAMBATTLE_INFO";
		}

		public override byte GetId()
		{
			return 0x9b;
		}*/

		protected override void WriteImpl()
		{
			// Totally unknown
			// 12 Ints
			WriteInt(0); 
			WriteInt(1);
			WriteInt(2); // Kills 
			WriteInt(3); // Assists
			WriteInt(4); // Aerogate (captures?)
			WriteInt(5); 
			WriteInt(6); // Deaths
			WriteInt(7); // Wins
			WriteInt(8); // Losses
			WriteInt(9); // Draws
			WriteInt(10); // Desertions
			WriteInt(11); // Points
			WriteInt(12); // High score
			WriteInt(3600); // Time
			WriteInt(0); // 
		}
	}
}