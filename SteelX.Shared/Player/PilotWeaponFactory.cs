namespace SteelX.Shared
{
	/// <summary>
	/// As a pilot gains levels in certain weapon classes, 
	/// he or she gains bonuses to those particular weapons. 
	/// These bonuses can included greater damage, faster reload times, etc.
	/// </summary>
	public abstract class PilotWeaponFactory
	{
		#region Variable
		public uint PlayerId				{ get; private set; }

		#region Weapon Progression
		/// <summary>
		/// Melee (Sword and Spear) Level
		/// </summary>
		public abstract byte FightLevel		{ get; }
		public uint FightExperience			{ get; protected set; }
		/// <summary>
		/// Sniper (Range and Rectifier?) Level
		/// </summary>
		public abstract byte RangedLevel	{ get; }
		public uint RangedExperience		{ get; protected set; }
		/// <summary>
		/// Assault (SMG and Shotgun) Level
		/// </summary>
		public abstract byte SiegeLevel		{ get; }
		public uint SiegeExperience			{ get; protected set; }
		/// <summary>
		/// Artilary (Rocket and Cannon) Level
		/// </summary>
		public abstract byte RocketLevel	{ get; }
		public uint RocketExperience		{ get; protected set; }
		/// <summary>
		/// Booster Level
		/// </summary>
		public abstract byte BackpackLevel	{ get; }
		public uint BackpackExperience		{ get; protected set; }
		#endregion

		#region Weapon Growth
		public float SwordOverheatBonus			{ get; protected set; }
		public float SwordDamageBonus			{ get; protected set; }
		public float SwordSplashDamageBonus		{ get; protected set; }
		public float SpearOverheatBonus			{ get; protected set; }
		public float SpearDamageBonus			{ get; protected set; }

		public float RectifierOverheatBonus		{ get; protected set; }
		public float RectifierDamageBonus		{ get; protected set; }
		public float RectifierHitMinBonus		{ get; protected set; }
		public float RifleOverheatBonus			{ get; protected set; }
		public float RifleDamageBonus			{ get; protected set; }

		public float ShotgunOverheatBonus		{ get; protected set; }
		public float ShotgunDamageBonus			{ get; protected set; }
		public float ShotgunHitMinBonus			{ get; protected set; }
		public float SMGOverheatBonus			{ get; protected set; }
		public float SMGDamageBonus				{ get; protected set; }

		public float CannonOverheatBonus		{ get; protected set; }
		public float CannonDamageBonus			{ get; protected set; }
		public int	 CannonSplashCountBonus		{ get; protected set; }
		public float RocketOverheatBonus		{ get; protected set; }
		public float RocketDamageBonus			{ get; protected set; }

		public float BoosterWeighttBonus		{ get; protected set; }
		public float BoosterHPBonus				{ get; protected set; }
		public float BoosterBoostPowerBonus		{ get; protected set; }
		#endregion
		#endregion

		#region Abstract Methods
		// Create methods here that convert experience points
		// to bonuses that are distributed across stats
		// One version for Server (Receive Command), 
		// another for Client (Send Command)
		#endregion
	}
}