using System;
using System.Linq;
using SteelX.Shared;
//using System.Drawing;
//using Console = Colorful.Console;

namespace SteelX.Client.Packets.Game
{
	/// <summary>
	/// Called when the client enters sniper mode
	/// </summary>
	public class ModeSniper : ClientGameBasePacket
	{
		public ModeSniper(byte[] data, GameSession client) : base(data, client)
		{
			TickUnit();
			
			// Read the units position and aim
			GetUnitPositionAndAim();
			
			Console.WriteLine("Sniper unknown byte {0}", GetByte());
		}

		public override Shared.PacketTypes PacketType
		{
			get
			{
				return Shared.PacketTypes.MODE_SNIPER;
			}
		}

		/*public override string GetType()
		{
			return "MODE_SNIPER";
		}*/

		protected override void RunImpl()
		{
			//GetClient().GameInstance.EnterSniperMode(Unit);

			//Bool toggle for if the player is using the weapon's scope zoom
			//Server checks if the weapon has the ability to zoom
		}
	}
}