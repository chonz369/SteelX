using SteelX.Shared;
using SteelX.Server;
//using Data.Model;

namespace SteelX.Server.Packets
{
	/// <summary>
	/// Probably sent as a response to buying an operator or selecting one
	/// </summary>
	public class SelectOperatorInfo : ServerBasePacket
	{
		// TODO: Pull actual operator from inv and handle result codes
		private readonly uint _tempOp;

		public override Shared.PacketTypes PacketType
		{
			get
			{
				return Shared.PacketTypes.SELECT_OPERATOR_INFO;
			}
		}

		public SelectOperatorInfo(uint id)
		{
			_tempOp = id;
		}

		/*public override string GetType()
		{
			return "SELECT_OPERATOR_INFO";
		}

		public override byte GetId()
		{
			return 0xe6;
		}*/

		protected override void WriteImpl()
		{
			WriteInt(0); // Result
			WriteUInt(_tempOp); // Id
		}
	}
}