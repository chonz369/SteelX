namespace SteelX.Shared
{
	/// <summary>
	/// Channel structure renewal ability penalty data
	/// </summary>
	public struct ChannelPenalty
	{
		public int ChannelPenaltyTemplateId { get; private set; }
		public int ChannelPenalty_HP { get; private set; }
		public int ChannelPenalty_ScanRange { get; private set; }
		public int ChannelPenalty_Endurance { get; private set; }
		public int ChannelPenalty_EN { get; private set; }
		public int ChannelPenalty_MoveSpeed { get; private set; }
		public int ChannelPenalty_BoostSpeed { get; private set; }
		public int ChannelPenalty_Damage { get; private set; }
	}
}