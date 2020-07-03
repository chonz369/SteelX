using SteelX.Shared;

namespace SteelX.Server.Packets.Inventory
{
	/// <summary>
	/// Sends the ID of the users default unit
	/// Maybe for lobby screen?
	/// </summary>
	public class SendDefaultUnit : ServerBasePacket
	{
		/// <summary>
		/// The id of the default unit
		/// </summary>
		private readonly uint _defaultId;
		
		public SendDefaultUnit(uint defaultId)
		{
			_defaultId = defaultId;
		}

		public override Shared.PacketTypes PacketType
		{
			get
			{
				return Shared.PacketTypes.INV_SEND_DEFAULT_UNIT;
			}
		}

		/*public override string GetType()
		{
			return "INV_SEND_DEFAULT_UNIT";
		}

		public override byte GetId()
		{
			return 0x1e;
		}*/

		protected override void WriteImpl()
		{
			WriteUInt(_defaultId);
		}
	}
}