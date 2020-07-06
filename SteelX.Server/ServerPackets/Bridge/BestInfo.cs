using SteelX.Shared;
using SteelX.Server;

namespace SteelX.Server.Packets.Bridge
{   
	/// <summary>
	/// A packet containing stats for a particular mode of play
	/// </summary>
	public class BestInfo : ServerBasePacket
	{
		/// <summary>
		/// The user this stat info is for
		/// </summary>
		private readonly Player _user;
		
		public BestInfo(Player user)
		{
			_user = user;
		}

		public override Shared.PacketTypes PacketType
		{
			get
			{
				return Shared.PacketTypes.BRIDGE_SEND_BEST_INFO;
			}
		}

		/*public override string GetType()
		{
			return $"BRIDGE_SEND_BEST_INFO";
		}

		public override byte GetId()
		{
			return 0x9f;
		}*/

		protected override void WriteImpl()
		{
			// Overall has a mysterious string and int header
			WriteInt(0);
			WriteString("OverallInfo");
			
			// Totally unknown
			// 12 Ints
			WriteInt(1000);
			WriteInt(2000);
			WriteInt(3000); 
			WriteInt(4000); 
			WriteInt(5); // Kills
			WriteInt(6); // Deaths
			WriteInt(102); // Assist
			WriteInt(8000);
			WriteInt(100); // Wins
			WriteInt(101); // Lose
			WriteInt(11); // Points
			WriteInt(103);// Best
			WriteInt(13000);
		}
	}
}