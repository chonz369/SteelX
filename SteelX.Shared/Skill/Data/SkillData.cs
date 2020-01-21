namespace SteelX.Shared
{
	public struct SkillData
	{
		public int Image			{ get; private set; }
		public string Name			{ get; private set; }
		public string Weap1			{ get; private set; }
		public string Weap2			{ get; private set; }
		public int Level			{ get; private set; }
		public int Price			{ get; private set; }
		public byte RequiredRank	{ get; private set; }
		public string Period		{ get; private set; }
		public byte Damage			{ get; private set; }
		public byte Radius			{ get; private set; }
		public byte Range			{ get; private set; }
		public int RequiredSP		{ get; private set; }
		public int CoolDown			{ get; private set; }
		public Skills Id			{ get; private set; }
		//public Weapon[] Weapon
	}
}