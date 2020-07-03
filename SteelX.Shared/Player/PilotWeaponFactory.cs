namespace SteelX.Shared
{
	public abstract class PilotWeaponFactory
	{
		#region Variable
		public uint PlayerId			{ get; private set; }
		public WeaponTypes WeaponType	{ get; private set; }

		#region Weapon Progression
		/// <summary>
		/// Melee (Sword and Spear) Level
		/// </summary>
		public byte FightLevel { get; protected set; }
		/// <summary>
		/// Sniper (Range and Rectifier?) Level
		/// </summary>
		public byte RangedLevel { get; protected set; }
		/// <summary>
		/// Assault (SMG and Shotgun) Level
		/// </summary>
		public byte SiegeLevel { get; protected set; }
		/// <summary>
		/// Artilary (Rocket and Cannon) Level
		/// </summary>
		public byte RocketLevel { get; protected set; }
		/// <summary>
		/// Booster Level
		/// </summary>
		public byte BackpackLevel { get; protected set; }
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
		// Create methods here that subtract points
		// in exchange for progression.
		// One version for Server (Receive Command), 
		// another for Client (Send Command)
		#endregion
	}
}