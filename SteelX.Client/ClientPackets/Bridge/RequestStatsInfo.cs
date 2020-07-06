using System;
using System.Linq;
using SteelX.Shared;
using SteelX.Shared.Packets;
//using Data.Model;
//using GameServer.ServerPackets;
//using GameServer.ServerPackets.Bridge;
//using GameServer.ServerPackets.Bridge.Stats;

namespace SteelX.Client.Packets.Bridge
{
	/// <summary>
	/// When the user requests their stats
	/// </summary>
	public class RequestStatsInfo : ClientBasePacket
	{
		public override Shared.PacketTypes PacketType
		{
			get
			{
				return Shared.PacketTypes.BRIDGE_REQ_STAT_INFO;
			}
		}

		public RequestStatsInfo(byte[] data, GameSession client) : base(data, client)
		{
		}

		/*public override string GetType()
		{
			return "BRIDGE_REQ_STAT_INFO";
		}*/

		protected override void RunImpl()
		{
			// Determine the stat type based on the Id offset
			/*var statType = (StatTypes)(Id - 0x47);
			
			// Lookup stats
			var stats = GetClient().User.Stats.FirstOrDefault(s => s.Type == statType);

			if (stats == null)
			{
				Console.WriteLine("Unable to find stat type of {0} on user!", statType);
				return;
			}

			ServerBasePacket packet;

			switch (statType)
			{
				case StatTypes.Training:
					packet = new TrainingInfo(stats);
					break;
				case StatTypes.Survival:
					packet = new SurvivalInfo(stats);
					break;
				case StatTypes.TeamSurvival:
					packet = new TeamSurvivalInfo(stats);
					break;
				case StatTypes.TeamBattle:
					packet = new TeamBattleInfo(stats);
					break;
				case StatTypes.Ctf:
					packet = new CtfInfo(stats);
					break;
				case StatTypes.ClanBattle:
					packet = new ClanBattleInfo(stats);
					break;
				case StatTypes.DefensiveBattle:
					packet = new DefensiveBattleInfo(stats);
					break;
				default:
					return;
			}
			
			GetClient().SendPacket(packet);*/

			//Sends a ping to server with type of stats
			//server will respond with the player's progress for given stat
		}
	}
}