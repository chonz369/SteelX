using System;
using SteelX.Shared;
//using GameServer.ServerPackets;

namespace SteelX.Client.Packets
{
	/// <summary>
	/// Sent when the client wishes to select their operator
	/// </summary>
	public class SelectOperator : ClientBasePacket
	{
		/// <summary>
		/// The operator they wish to select
		/// </summary>
		private readonly uint _operatorId;

		public override Shared.PacketTypes PacketType
		{
			get
			{
				return Shared.PacketTypes.SELECT_OPERATOR;
			}
		}

		public SelectOperator(byte[] data, GameSession client) : base(data, client)
		{
			Console.WriteLine("INT - {0}", GetInt()); // Unknown

			_operatorId = GetUInt();
			
			Console.WriteLine("OperatorId - {0}", _operatorId);
		}

		/*public override string GetType()
		{
			return "SELECT_OPERATOR";
		}*/

		protected override void RunImpl()
		{
			//TODO: Actually select operator
			var client = GetClient();
			
			client.SendPacket(new SelectOperatorInfo(_operatorId));
		}
	}
}