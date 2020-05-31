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
		public static Dictionary<Parts, PartData> ArmsData { get; private set; }
		public static Dictionary<Parts, PartData> LegsData { get; private set; }
		public static Dictionary<Parts, PartData> CoresData { get; private set; }
		public static Dictionary<Parts, PartData> HeadsData { get; private set; }
		public static Dictionary<Parts, PartData> BoostersData { get; private set; }
		//public static Dictionary<Parts, PartData> WeaponsData { get; private set; }
		//public static Dictionary<Parts, PartData> SkillsData { get; private set; }
	}
}
