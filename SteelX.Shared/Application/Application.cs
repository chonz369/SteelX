using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SteelX.Shared;

namespace SteelX
{
	public partial class Application
	{
		public static Dictionary<Parts, PartData> PartsData { get; private set; }
		public static Dictionary<Parts, ArmData> ArmsData { get; private set; }
		public static Dictionary<Parts, LegData> LegsData { get; private set; }
		public static Dictionary<Parts, CoreData> CoresData { get; private set; }
		public static Dictionary<Parts, HeadData> HeadsData { get; private set; }
		public static Dictionary<Parts, BoosterData> BoostersData { get; private set; }
		//public static Dictionary<Weaponz, WeaponData> WeaponsData { get; private set; }
		//public static Dictionary<Skills, SkillData> SkillsData { get; private set; }

		public static Dictionary<Weaponz, Weapon> WeaponsFactory { get; private set; }
		public static Dictionary<Skills, Skill> SkillsFactory { get; private set; }


		//public static bool InitParts(bool? sql = null)
		//{
		//	PartsData = new Dictionary<Parts, PartData>();
		//	if (sql.Value == true) //using (con)
		//		return GetPokemonsFromSQL(con);
		//	else return GetPokemonsFromXML();
		//}
	}
}
