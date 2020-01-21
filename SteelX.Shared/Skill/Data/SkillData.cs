namespace SteelX.Shared
{
	public struct SkillData
	{
		public string Image { get; private set; }
		//public string Name    		{ get; private set; }
		public string LeftHand { get; private set; }
		public string RightHand { get; private set; }
		public string Level { get; private set; }
		public string Price { get; private set; }
		public string RequiredRank { get; private set; }
		public string Period { get; private set; }
		public string Damage { get; private set; }
		public string Radius { get; private set; }
		public string Range { get; private set; }
		public string RequiredSP { get; private set; }
		public int CoolDown { get; private set; }
		public Skills Id { get; private set; }
		//public Weapon[] Weapon
	}
}