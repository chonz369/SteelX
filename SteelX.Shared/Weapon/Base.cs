namespace SteelX.Shared
{
	/// <summary>
	/// </summary>
	//ToDo: In-Game weapons should use [2][2] (to track ammo, and other misc data)
	//Use bool to check for Weapon[n][IsEquipped && IsTwoHanded]
	public class Weapon
	{
		#region Variables
		public Weaponz WeaponId { get; private set; }
		public WeaponTypes Class { get { return GetWeapType(WeaponId); } }


		/// <summary>
		/// Name of the set, this part belongs to
		/// </summary>
		public string SetName { get; set; }
		/// <summary>
		/// Internal name of this individual part
		/// </summary>
		public string PartName { get; set; }
		/// <summary>
		/// Name of this this part when viewed in-game from shop or inventory
		/// </summary>
		public string DisplayName { get; set; }
		public string ImagePath { get { return string.Format("{0}", PartName); } }
		/// <summary>
		/// The name of the sprite asset in UnityEngine
		/// </summary>
		/// Either this or ImagePath would be the best way to handle loading Icon
		public string ImageSpriteAsset { get; private set; }

		public int Durability	{ get; private set; }
		public int Weight		{ get; private set; }
		public int Size			{ get; private set; }
		public float Range		{ get; private set; }
		public float Damage		{ get; private set; }
		public float Accuracy	{ get; private set; }
		public float Reload		{ get; private set; }
		public float Overheat	{ get; private set; }
		public int BuyPrice		{ get; private set; }
		/// <summary>
		/// </summary>
		/// ToDo: When in hands of player, price is buy multiplied by (durability divided by max durability)
		/// An Array of this Unit's Price
		/// If value is null, then option does not exist
		/// Index[1] : Credits
		/// Index[2] : Coins
		public virtual int SellPrice	{ get { return 0; } } //abstract
		/// <summary>
		/// Rank required to purchase or equip part
		/// </summary>
		public byte RankRequired { get; set; }
		/// <summary>
		/// 
		/// </summary>
		public int Description { get; set; }
		#endregion

		#region Constructor
		public Weapon(Weaponz weap)
		{
			//Weapon weapon = GetWeapon(weap);
			//PartName = weapon.PartName;
			//DisplayName = weapon.DisplayName;
			//RankRequired = weapon.RankRequired;
			//HP = weapon.HP;
			//Size = weapon.Size;
			//Weight = weapon.Weight;
			//EnergyDrain = weapon.EnergyDrain;
			//MaxHeat = weapon.MaxHeat;
			//CooldownRate = weapon.CooldownRate;
			//Marksmanship = weapon.Marksmanship;
			//WeightSeries = weapon.WeightSeries;
		}
		public Weapon(Weaponz? name = null, string display = null, byte rank = 0, int hp = 0, int size = 0, int weight = 0, int endrain = 0, int heat = 0, int cooldown = 0, int mark = 0, WeightClass? type = null)
		{
			//PartName = name.HasValue ? name.Value : Weapon.NONE;
			//DisplayName = display ?? "GameMaster";
			//RankRequired = rank;
			//HP = hp;
			//Size = size;
			//Weight = weight;
			//EnergyDrain = endrain;
			//MaxHeat = heat;
			//CooldownRate = cooldown;
			//Marksmanship = mark;
			//WeightSeries = type.HasValue ? type.Value : WeightClass.Standard;
		}
		#endregion

		#region Methods
		internal Weapon GetWeapon(Weaponz ID)
		{
			foreach (Weapon weap in Database)
			{
				if (weap.WeaponId == ID) return weap;
			}
			throw new System.Exception("Part ID doesnt exist in the database. Please check Arms constructor.");
		}
		public static WeaponTypes GetWeapType(Weaponz weap)
		{
			//if (WeaponId == Weapon.NONE)
			//	return WeaponType.NONE;
			//else 
			switch (weap)
			{
				case Weaponz.NONE:
				default:
					return WeaponTypes.NONE;
			}
		}
		#endregion

		#region Database
		private static readonly Weapon[] Database;
		static Weapon()
		{
			Database = new Weapon[] {
			};
		}
		#endregion
	}
}