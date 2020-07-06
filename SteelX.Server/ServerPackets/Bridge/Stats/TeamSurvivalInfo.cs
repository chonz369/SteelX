using SteelX.Shared;
using SteelX.Server;

namespace SteelX.Server.Packets.Bridge.Stats
{   
	/// <summary>
	/// A packet containing stats for team survival (team deathmatch)
	/// </summary>
	public class TeamSurvivalInfo : ServerBasePacket
	{
		private readonly UserStats _stats;
		
		public TeamSurvivalInfo(UserStats stats)
		{
			_stats = stats;
		}

		public override Shared.PacketTypes PacketType
		{
			get
			{
				return Shared.PacketTypes.BRIDGE_SEND_TEAMSURVIVAL_INFO;
			}
		}

		/*public override string GetType()
		{
			return $"BRIDGE_SEND_TEAMSURVIVAL_INFO";
		}

		public override byte GetId()
		{
			return 0x9a;
		}*/

		protected override void WriteImpl()
		{
			// Totally unknown
			// 12 Ints
			WriteInt(0); 
			WriteInt(0);// 
			WriteInt(2); // Kills
			WriteInt(3); // Assists
			WriteInt(0); // 
			WriteInt(5); // Deaths
			WriteInt(6); // Wins
			WriteInt(7); // Losses
			WriteInt(8); // Draws
			WriteInt(9); // Desertions
			WriteInt(10); // Points
			WriteInt(11); // High Score
			WriteInt(3600); // Time
		}
	}
}