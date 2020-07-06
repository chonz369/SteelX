namespace SteelX.Shared
{
	/// <summary>
	/// Represents a single weapon in the game
	/// </summary>
	/// Mostly used in runtime
	public class Weapon : Part
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
		//public int BuyPrice		{ get; private set; }
		/// <summary>
		/// </summary>
		/// ToDo: When in hands of player, price is buy multiplied by (durability divided by max durability)
		/// An Array of this Unit's Price
		/// If value is null, then option does not exist
		/// Index[1] : Credits
		/// Index[2] : Coins
		//public virtual int SellPrice	{ get { return 0; } } //abstract
		/// <summary>
		/// Rank required to purchase or equip part
		/// </summary>
		public byte RankRequired		{ get; set; }
		/// <summary>
		/// </summary>
		public int Description			{ get; set; }

		/// <summary>
		/// The target of this weapon
		/// </summary>
		public uint? Target				{ get; set; }

		/// <summary>
		/// The current level of overheat this weapon has
		/// </summary>
		public float CurrentOverheat	{ get; private set; }

		// From weapon
		public float OverheatPerShot	{ get; private set; }
		// From weapon
		public float NormalRecovery		{ get; private set; }
		// From arms of unit
		public float OverheatRecovery	{ get; private set; } 
		// From arms of unit
		public float MaxOverheat		{ get; private set; }

		// Automatic weapons
		public bool IsAutomatic			{ get { return Class == WeaponTypes.SMGs; } }
		// Used for automatic weapons
		public float ReloadTime			{ get; private set; }
		public bool IsAttacking			{ get; protected set; }
		public float CurrentReloadTime	{ get; private set; }
		public int NumberOfShots		{ get; private set; }

		/// <summary>
		/// Is this weapon overheated?
		/// </summary>
		public bool IsOverheated		{ get; private set; }
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

		/// <summary>
		/// Called when the weapon is fired to increase its reload timer
		/// </summary>
		public void AddReloadTime()
		{
			//Console.WriteLine("Added {0} reload time", ReloadTime);
			CurrentReloadTime = ReloadTime;
		}

		/// <summary>
		/// Checks if the weapon is ready to fire again
		/// </summary>
		/// <param name="delta"></param>
		/// <returns></returns>
		public bool ShouldAttack(float delta)
		{
			if (IsAttacking)
			{
				if (CurrentReloadTime <= 0)
				{
					// Yes, attack
					return true;
				}
				else
				{
					CurrentReloadTime = CurrentReloadTime - ReloadTime * delta / 1000;
				}
			}

			return false;
		}

		/// <summary>
		/// Adds overheat to the weapon
		/// </summary>
		//TODO: Calculate this
		public bool AddOverheat()
		{
			CurrentOverheat += OverheatPerShot;

			//TODO: From unit config?
			if (CurrentOverheat >= MaxOverheat)
			{
				IsOverheated = true;
				IsAttacking = false;
				CurrentOverheat = MaxOverheat;

				return true;
			}

			return false;
		}

		/// <summary>
		/// Tick overheat and see if we need to update the client
		/// </summary>
		/// <param name="delta">Time in MS since last tick</param>
		public bool ShouldUpdateOverheat(float delta)
		{
			if (CurrentOverheat > 0)
			{
				CurrentOverheat -= (IsOverheated ? OverheatRecovery : NormalRecovery) * delta / 1000;

				if (CurrentOverheat <= 0)
				{
					CurrentOverheat = 0;

					if (IsOverheated)
					{
						IsOverheated = false;

						// Do update
						return true;
					}
				}
			}

			// Do not update
			return false;
		}
		#endregion
	}
}