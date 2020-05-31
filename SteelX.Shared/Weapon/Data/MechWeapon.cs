namespace SteelX.Shared
{
	public struct MechWeapon
	{
		public Weaponz LH	{ get; set; }
		public Weaponz RH	{ get; set; }
		public bool IsTwoHanded	{ get { return LH == RH & (this == WeaponTypes.Rockets || this == WeaponTypes.Rifles); } }

		#region Explicit Operators
		public static bool operator == (MechWeapon equip, WeaponTypes weap)
		{
			return weap == Weapon.GetWeapType(equip.LH != Weaponz.NONE ? equip.LH : equip.RH);
		}
		public static bool operator != (MechWeapon equip, WeaponTypes weap)
		{
			return weap != Weapon.GetWeapType(equip.LH != Weaponz.NONE ? equip.LH : equip.RH);
		}
		//public static bool operator == (Weapon lh, Weapon rh)
		//public static bool operator == (MechWeapons lh, MechWeapons rh)
		//{
		//	return true;
		//}
		//public static bool operator != (MechWeapons lh, MechWeapons rh)
		//{
		//	return true;
		//}
		public override bool Equals(object obj)
		{
			return base.Equals(obj);
		}
		public override int GetHashCode()
		{
			return base.GetHashCode();
		}
		#endregion
	}
}