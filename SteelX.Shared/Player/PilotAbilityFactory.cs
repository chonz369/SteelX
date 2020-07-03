namespace SteelX.Shared
{
	public abstract class PilotAbilityFactory
	{
		#region Variable
		public uint PlayerId { get; set; }

		/// <summary>
		/// The number of points the user has available to spend
		/// </summary>
		/// <remarks>
		/// You are able to incorporate and upgrade many attributes to your mech as you gain levels and ranks. 
		/// You gain 3 "Pilot Points" per level. 
		/// Each stat costs 2 Pilot Points to increase and has a maximum level of 30, 
		/// aside from the Marksmanship stat.
		/// </remarks>
		public int AbilityPointsAvailable { get; protected set; }

		#region Ability Progression
		/// <summary>
		/// Increase HP
		/// </summary>
		/// <remarks>
		/// Increases your HP by 10 points per level. 
		/// Increasing this stat helps you live longer in game, 
		/// enabling you to survive longer in hostile conditions. 
		/// After level 10 of this abiltiy, it will increase by 20 each level.
		/// </remarks>
		/// Levels 1-10		2 Pilot Points	+10 HP
		/// Increase HP
		/// Levels 11-20	3 Pilot Points	+20 HP
		/// Increase HP
		/// Levels 21-30	4 Pilot Points	+20 HP
		/// Increase HP
		/// Levels 31-40	5 Pilot Points	+70 HP
		public byte HpLevel { get; protected set; }
		public int HpBonus { get { return 0; } }
		/// <summary>
		/// Increase Movement Speed
		/// </summary>
		/// <remarks>
		/// Increases your Base Speed by 1 point per level. 
		/// Increasing this stat increases your move speed as well as your boosting speed. 
		/// Maximum level is 20.
		/// </remarks>
		/// Odd Levels		2 Pilot Points	+1 Movement Speed
		/// Even Levels		2 Pilot Points	+1 Movement Speed; +1 Dash Speed
		public byte MoveSpeedLevel { get; protected set; }
		public int MoveSpeedBonus { get { return 0; } }
		public int DashSpeedBonus { get { return 0; } }
		/// <summary>
		/// Increase EN
		/// </summary>
		/// <remarks>
		/// Increases your EN by 10 points per level. 
		/// Increasing this stat enables you to use any action 
		/// that requires EN more frequently such as boosting and changing weapons.
		/// </remarks>
		/// Levels 1-10		2 Pilot Points	+10 EN
		/// Increase EN
		/// Levels 11-20	3 Pilot Points	+20 EN
		/// Increase EN
		/// Levels 21-30	4 Pilot Points	+20 EN
		/// Increase EN
		/// Levels 31-40	5 Pilot Points	+20 EN
		public byte EnLevel { get; protected set; }
		public int EnBonus { get { return 0; } }
		/// <summary>
		/// Increase Scan Range
		/// </summary>
		/// <remarks>
		/// Increases your Scan Range by 12 points per level. 
		/// Increasing this stat increases the range of your radar, 
		/// enabling your mech to be able to detect enemies and allies farther away.
		/// </remarks>
		/// Levels 1-7		2 Pilot Points	+12 Scan Range
		public byte ScanRangeLevel { get; protected set; }
		public int ScanRangeBonus { get { return 0; } }
		/// <summary>
		/// Increase SP
		/// </summary>
		/// <remarks>
		/// Increases your SP by 10 points per level. 
		/// Increasing this skill enables you to execute skills 
		/// that you have added to your mech more frequently within the game.
		/// </remarks>
		/// Levels 1-20		? Pilot Points	+10 SP
		public byte SpLevel { get; protected set; }
		public int SpBonus { get { return 0; } }
		/// <summary>
		/// Increase Marksmanship
		/// </summary>
		/// <remarks>
		/// Increases your Marksmanship by 5 points per level. 
		/// The maximum level for this stat is 5. 
		/// Each level costs 3 times the level Pilot Points 
		/// (i.e. Level 5 costs 15 Pilot Points to obtain, 
		/// meaning it costs 45 Pilot Points to master this stat.)
		/// </remarks>
		/// Level 1-5	3*Level Pilot Points per Level
		/// 3,6,9,12,15	+5 Marksmanship
		public byte AimLevel { get; protected set; }
		public int AimBonus { get { return 0; } }
		#endregion

		//As a pilot gains levels in certain weapon classes, 
		//he or she gains bonuses to those particular weapons. 
		//These bonuses can included greater damage, faster reload times, etc.
		/*#region Weapon Progression
		/// <summary>
		/// Melee (Sword and Spear) Level
		/// </summary>
		public byte FightLevel { get; }
		public uint FightExperience { get; protected set; }
		/// <summary>
		/// Sniper (Range and Rectifier?) Level
		/// </summary>
		public byte RangedLevel { get; }
		public uint RangedExperience { get; protected set; }
		/// <summary>
		/// Assault (SMG and Shotgun) Level
		/// </summary>
		public byte SiegeLevel { get; }
		public uint SiegeExperience { get; protected set; }
		/// <summary>
		/// Artilary (Rocket and Cannon) Level
		/// </summary>
		public byte RocketLevel { get; }
		public uint RocketExperience { get; protected set; }
		/// <summary>
		/// Booster Level
		/// </summary>
		public byte BackpackLevel { get; }
		public uint BackpackExperience { get; protected set; }
		#endregion

		#region Weapon Growth
		public float SwordOverheatBonus { get; protected set; }
		public float SwordDamageBonus { get; protected set; }
		public float SwordSplashDamageBonus { get; protected set; }
		public float SpearOverheatBonus { get; protected set; }
		public float SpearDamageBonus { get; protected set; }

		public float RectifierOverheatBonus { get; protected set; }
		public float RectifierDamageBonus { get; protected set; }
		public float RectifierHitMinBonus { get; protected set; }
		public float RifleOverheatBonus { get; protected set; }
		public float RifleDamageBonus { get; protected set; }

		public float ShotgunOverheatBonus { get; protected set; }
		public float ShotgunDamageBonus { get; protected set; }
		public float ShotgunHitMinBonus { get; protected set; }
		public float SMGOverheatBonus { get; protected set; }
		public float SMGDamageBonus { get; protected set; }

		public float CannonOverheatBonus { get; protected set; }
		public float CannonDamageBonus { get; protected set; }
		public int CannonSplashCountBonus { get; protected set; }
		public float RocketOverheatBonus { get; protected set; }
		public float RocketDamageBonus { get; protected set; }

		public float BoosterWeighttBonus { get; protected set; }
		public float BoosterHPBonus { get; protected set; }
		public float BoosterBoostPowerBonus { get; protected set; }
		#endregion*/
		#endregion

		#region Abstract Methods
		// Create methods here that subtract points
		// in exchange for progression.
		// One version for Server (Receive Command), 
		// another for Client (Send Command)
		#endregion
	}
}