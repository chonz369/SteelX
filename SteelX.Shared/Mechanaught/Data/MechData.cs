using SteelX.Shared;

namespace SteelX.Shared
{
	public struct MechData
	{
		public Parts Arms		{ get; set; }
		public Parts Legs		{ get; set; }
		public Parts Core		{ get; set; }
		public Parts Head		{ get; set; }
		public Parts Booster	{ get; set; }
		public Parts ArmsColor		{ get; set; }
		public Parts LegsColor		{ get; set; }
		public Parts CoreColor		{ get; set; }
		public Parts HeadColor		{ get; set; }
		public Parts BoosterColor	{ get; set; }
		//public Weapon Weapon	{ get; set; }
		public MechWeapon[] WeaponSet	{ get; set; }
		//ToDo: Mech Colors
		public Skills[] Skills	{ get; set; }
	}
}