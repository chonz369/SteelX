using SteelX.Shared;
using SteelX.Server;

namespace SteelX.Server.Packets.Bridge.Stats
{   
	/// <summary>
	/// A packet containing stats for survival (deathmatch)
	/// </summary>
	public class DefensiveBattleInfo : ServerBasePacket
	{
		private readonly UserStats _stats;
		
		public DefensiveBattleInfo(UserStats stats)
		{
			_stats = stats;
		}

		public override Shared.PacketTypes PacketType
		{
			get
			{
				return Shared.PacketTypes.BRIDGE_SEND_DEFENSIVEBATTLE_INFO;
			}
		}

		/*public override string GetType()
		{
			return $"BRIDGE_SEND_DEFENSIVEBATTLE_INFO";
		}

		public override byte GetId()
		{
			return 0x9e;
		}*/

		protected override void WriteImpl()
		{
			// Totally unknown
			// 8 Ints
			WriteInt(0); // 
			WriteInt(0); // 
			WriteInt(100); // Wins
			WriteInt(101); // Desertions
			WriteInt(102); // Losses
			WriteInt(103); // Points
			WriteInt(104); // NPC Kills
			WriteInt(3600); // Time
			
			
			WriteInt(1); // Size ?
			
			WriteInt(2); // Map ID
			WriteInt(1); // Total record (points?)
			WriteInt(2); // Total ranking (leaderboard?)
			WriteInt(3); // Weekly record
			WriteInt(4); // Weekly ranking
			WriteInt(5); // Last weeks record
			WriteInt(6); // Last weeks ranking
			WriteInt(7); // Last weeks credits earned
		}
	}
}