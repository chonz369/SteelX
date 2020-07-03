using SteelX.Shared.Utility;

namespace SteelX.Shared
{
	public class Mechanaught
	{
		#region Variables
		/// <summary>
		/// Unique Id for this unit.
		/// </summary>
		public uint Id              { get; private set; }
		public PilotAbilityFactory PilotAbility	{ get; private set; }
		//public PilotWeaponFactory PilotWeapon	{ get; private set; }
		public Parts Arm			{ get; private set; }
		public Parts Leg			{ get; private set; }
		public Parts Core			{ get; private set; }
		public Parts Head			{ get; private set; }
		public Parts Booster		{ get; private set; }
		public Weapon[,] Weapons	{ get; private set; }
		public Skills[] Skills		{ get; private set; }

		/// <summary>
		/// The inventory this unit is part of
		/// </summary>
		public uint UserInventoryId { get; set; }

		/// <summary>
		/// The team this unit is on
		/// </summary>
		/// If leave DM game and return, 
		/// use this to continue player score?
		public byte Team { get; protected set; }

		/// <summary>
		/// The launch order position of this unit
		/// </summary>
		public int LaunchOrder { get; set; }

		/// <summary>
		/// The name of this unit
		/// </summary>
		public string Name { get; set; }

		/// <summary>
		/// The head for this unit
		/// </summary>
		//public Part Head { get; set; }
		public uint HeadId { get; set; }

		/// <summary>
		/// The chest for this unit
		/// </summary>
		public Part Chest { get; set; }
		public uint ChestId { get; set; }

		/// <summary>
		/// The arms for this unit
		/// </summary>
		public Part Arms { get; set; }
		public uint ArmsId { get; set; }

		/// <summary>
		/// The legs for this unit
		/// </summary>
		public Part Legs { get; set; }
		public uint LegsId { get; set; }

		/// <summary>
		/// The backpack for this unit
		/// </summary>
		public Part Backpack { get; set; }
		public uint BackpackId { get; set; }

		/// <summary>
		/// The left weapon for this units first weapon set
		/// </summary>
		public Weapon WeaponSetLeft { get; set; }
		public uint WeaponSetLeftId { get; set; }

		/// <summary>
		/// The right weapon for this units first weapon set
		/// </summary>
		public Weapon WeaponSetRight { get; set; }
		public uint WeaponSetRightId { get; set; }

		/*// <summary>
		/// The left weapon for this units second weapon set
		/// </summary>
		public Weapon WeaponSet2Left { get; set; }
		public uint WeaponSet2LeftId { get; set; }

		/// <summary>
		/// The right weapon for this units second weapon set
		/// </summary>
		public Weapon WeaponSet2Right { get; set; }
		public uint WeaponSet2RightId { get; set; }*/

		/// <summary>
		/// The position of this unit in the world
		/// </summary>
		public Vector WorldPosition { get; set; }

		// TODO: Possibly replace X and Y with some sort of vector?

		/// <summary>
		/// The x aim of this unit
		/// </summary>
		public short AimX { get; set; }

		/// <summary>
		/// The y aim of this unit
		/// </summary>
		public short AimY { get; set; }

		/// <summary>
		/// Byte flag or enum from client showing movement
		/// </summary>
		/// Direction?
		//TODO: Find out if this is a flag or an enum
		public byte Movement { get; set; }

		/// <summary>
		/// Unknown byte or flag showing some sort of status for unit
		/// </summary>
		/// Falling, Jumping, Grounded?...
		//TODO: Find out if this is a flag or an enum
		public byte UnknownMovementFlag { get; set; }

		/// <summary>
		/// Byte flag from client showing booster status
		/// </summary>
		public bool Boosting { get; protected set; }

		/// <summary>
		/// Reflects user's current hp
		/// </summary>
		public int Health { get; protected set; }

		/// <summary>
		/// The current weapon set this user is using
		/// </summary>
		private int _currentWeaponSet = 0;

		/// <summary>
		/// Gets the current weapon set equipped
		/// </summary>
		public int CurrentWeaponSet { get { return _currentWeaponSet; } }

		public bool Alive { get { return Health > 0; } }

		#region Stats
		public int Durability		{ get { return Application.PartsData[Arm].Durability + Application.PartsData[Leg].Durability + Application.PartsData[Core].Durability + Application.PartsData[Head].Durability + Application.PartsData[Booster].Durability; } }
		public int Weight			{ get { return Application.PartsData[Arm].Weight + Application.PartsData[Leg].Weight + Application.PartsData[Core].Weight + Application.PartsData[Head].Weight + Application.PartsData[Booster].Weight; } } //- (int)(Application.PartsData[Booster].Weight * PilotWeapon.BoosterWeighttBonus)
		public int Size				{ get { return Application.PartsData[Arm].Size + Application.PartsData[Leg].Size + Application.PartsData[Core].Size + Application.PartsData[Head].Size + Application.PartsData[Booster].Size; } }
		public int HP				{ get { return Application.PartsData[Arm].HP + Application.PartsData[Leg].HP + Application.PartsData[Core].HP + Application.PartsData[Head].HP + Application.PartsData[Booster].HP + PilotAbility.HpBonus; } } //+ (int)(Application.PartsData[Booster].HP * PilotWeapon.BoosterHPBonus)
		public int EN				{ get { return Application.CoresData[Core].Capacity + PilotAbility.EnBonus; } }
		public int SP				{ get { return Application.HeadsData[Head].SPCache + PilotAbility.SpBonus; } }
		public int MPU				{ get { return Application.HeadsData[Head].MPU; } }
		public int MoveSpeed		{ get { return Application.LegsData[Leg].MoveSpeed + PilotAbility.MoveSpeedBonus; } }
		public int EN_Recovery		{ get { return Application.PartsData[Arm].RecoveryEN + Application.PartsData[Leg].RecoveryEN + Application.PartsData[Core].RecoveryEN + Application.PartsData[Head].RecoveryEN + Application.PartsData[Booster].RecoveryEN; } }
		public int MinEN_Required	{ get { return Application.CoresData[Core].MinSupply; } }
		#endregion
		#region Arms
		public int MaxHeat			{ get { return Application.ArmsData[Arm].MaxHeat; } }
		public int CooldownRate		{ get { return Application.ArmsData[Arm].CooldownRate; } }
		public int Marksmanship		{ get { return Application.ArmsData[Arm].Marksmanship + PilotAbility.AimBonus; } }
		#endregion
		#region Legs
		public int Capacity			{ get { return Application.LegsData[Leg].LoadAbility; } }
		public int Deceleration		{ get { return Application.LegsData[Leg].BreakAbility; } }
		#endregion
		#region Core
		public int ENOutputRate		{ get { return Application.CoresData[Core].Generate; } }
		public int EnergyDrain		{ get { return Application.PartsData[Arm].EnergyDrain + Application.PartsData[Leg].EnergyDrain + Application.PartsData[Core].EnergyDrain + Application.PartsData[Head].EnergyDrain + Application.PartsData[Booster].EnergyDrain; } }
		#endregion
		#region Head
		public int ScanRange		{ get { return Application.HeadsData[Head].ScanRange + PilotAbility.ScanRangeBonus; } }
		#endregion
		#region Booster
		public int DashOutput		{ get { return Application.BoostersData[Booster].BoosterPower; } } //+ (int)(Application.BoostersData[Booster].BoosterPower * PilotWeapon.BoosterBoostPowerBonus)
		public int DashENDrain		{ get { return Application.BoostersData[Booster].BoosterEnDrain; } }
		public int JumpENDrain		{ get { return Application.BoostersData[Booster].BoosterJumpEnDrain; } }
		#endregion
		public static readonly int OperatorLength = System.Enum.GetNames(typeof(OperatorStats)).Length;
		public int[] OperatorStats
		{
			get
			{
				int[] PropertiesArray = new int[OperatorLength];
				PropertiesArray[(byte)Shared.OperatorStats.HP]				= HP;
				PropertiesArray[(byte)Shared.OperatorStats.EN]				= EN;
				PropertiesArray[(byte)Shared.OperatorStats.SP]				= SP;
				PropertiesArray[(byte)Shared.OperatorStats.MPU]				= MPU;
				PropertiesArray[(byte)Shared.OperatorStats.Size]			= Size;
				PropertiesArray[(byte)Shared.OperatorStats.Weight]			= Weight;
				//PropertiesArray[6] = (int)mechProperty.GetMoveSpeed(partWeight, weaponWeight);
				//PropertiesArray[7] = (int)mechProperty.GetDashSpeed(partWeight + weaponWeight);
				PropertiesArray[(byte)Shared.OperatorStats.ENOutputRate]	= ENOutputRate;
				PropertiesArray[(byte)Shared.OperatorStats.MinEN_Required]	= MinEN_Required;//MinENRequired;
				PropertiesArray[(byte)Shared.OperatorStats.DashENDrain]		= DashENDrain;
				PropertiesArray[(byte)Shared.OperatorStats.JumpENDrain]		= JumpENDrain;//GetJumpENDrain(partWeight + weaponWeight);
				//PropertiesArray[12] = mechProperty.GetDashAcceleration(partWeight + weaponWeight);
				//PropertiesArray[13] = mechProperty.GetDashDecelleration(partWeight + weaponWeight);
				PropertiesArray[(byte)Shared.OperatorStats.MaxHeat]			= MaxHeat;
				PropertiesArray[(byte)Shared.OperatorStats.CooldownRate]	= CooldownRate;
				PropertiesArray[(byte)Shared.OperatorStats.ScanRange]		= ScanRange;
				PropertiesArray[(byte)Shared.OperatorStats.Marksmanship]	= Marksmanship;

				return PropertiesArray;
			}
		}
		#endregion

		#region Constructor
		public Mechanaught() 
		{
			Arm		= Parts.AGM001; //new Arms(Parts.AGM001);
			Leg		= Parts.LGM001; //new Legs(Parts.LGM001);
			Core	= Parts.CGM001; //new Cores(Parts.CGM001);
			Head	= Parts.HGM001; //new Heads(Parts.HGM001);
			Booster	= Parts.PGM001; //new Boosters(Parts.PGM001);
		}
		public Mechanaught(PilotAbilityFactory pilota, int[] mech) : this()//, PilotWeaponFactory pilotw
		{
			PilotAbility = pilota;
			//PilotWeapon = pilotw;
			//Arm = new Arms(mech.Arms);
			//Leg = new Legs(mech.Legs);
			//Core = new Cores(mech.Core);
			//Head = new Heads(mech.Head);
			//Booster = new Boosters(mech.Booster);
		}
		public Mechanaught LoadOut(MechWeapon weap, byte set = 1)
		{
			SetWeapons(weap, set);
			return this;
		}
		public void SetWeapons(MechWeapon weap, byte loadout = 1)
		{
			Weapons[loadout, 1] = new Weapon(weap.LH);
			Weapons[loadout, 2] = new Weapon(weap.RH);
		}
		#endregion
	}

	/// <summary>
	/// This class represents a unit in the game
	/// </summary>
	/// May need to be switched to NPC/User in the future
	//ToDo: Merge `Unit` with Mech class above, and remove
	public class Unit
	{
		/// <summary>
		/// Unique Id for this unit.
		/// </summary>
		/// Not sure if it is globally unique or just for the same session
		public uint Id { get; set; }

		/// <summary>
		/// The user who is owns this unit
		/// </summary>
		/// Not mapped for now as I think we might need to assign this at runtime, for NPC units and stuff
		public Player User { get; set; }

		/// <summary>
		/// The inventory this unit is part of
		/// </summary>
		public uint UserInventoryId { get; set; }

		/// <summary>
		/// The team this unit is on
		/// </summary>
		/// Only used in game?
		//public uint Team;

		/// <summary>
		/// The launch order position of this unit
		/// </summary>
		public int LaunchOrder { get; set; }

		/// <summary>
		/// The name of this unit
		/// </summary>
		public string Name { get; set; }

		/// <summary>
		/// The head for this unit
		/// </summary>
		public Part Head { get; set; }
		public uint HeadId { get; set; }

		/// <summary>
		/// The chest for this unit
		/// </summary>
		public Part Chest { get; set; }
		public uint ChestId { get; set; }

		/// <summary>
		/// The arms for this unit
		/// </summary>
		public Part Arms { get; set; }
		public uint ArmsId { get; set; }

		/// <summary>
		/// The legs for this unit
		/// </summary>
		public Part Legs { get; set; }
		public uint LegsId { get; set; }

		/// <summary>
		/// The backpack for this unit
		/// </summary>
		public Part Backpack { get; set; }
		public uint BackpackId { get; set; }

		/// <summary>
		/// The left weapon for this units first weapon set
		/// </summary>
		public Weapon WeaponSet1Left { get; set; }
		public uint WeaponSet1LeftId { get; set; }

		/// <summary>
		/// The right weapon for this units first weapon set
		/// </summary>
		public Weapon WeaponSet1Right { get; set; }
		public uint WeaponSet1RightId { get; set; }

		/// <summary>
		/// The left weapon for this units second weapon set
		/// </summary>
		public Weapon WeaponSet2Left { get; set; }
		public uint WeaponSet2LeftId { get; set; }

		/// <summary>
		/// The right weapon for this units second weapon set
		/// </summary>
		public Weapon WeaponSet2Right { get; set; }
		public uint WeaponSet2RightId { get; set; }

		/// <summary>
		/// The position of this unit in the world
		/// </summary>
		public Vector WorldPosition { get; set; }

		//TODO: Possibly replace X and Y with some sort of vector?

		/// <summary>
		/// The x aim of this unit
		/// </summary>
		public short AimX { get; set; }

		/// <summary>
		/// The y aim of this unit
		/// </summary>
		public short AimY { get; set; }

		/// <summary>
		/// Byte flag or enum from client showing movement
		/// </summary>
		//TODO: Find out if this is a flag or an enum
		public byte Movement { get; set; }

		/// <summary>
		/// Unknown byte or flag showing some sort of status for unit
		/// </summary>
		//TODO: Find out if this is a flag or an enum
		public byte UnknownMovementFlag { get; set; }

		/// <summary>
		/// Byte flag or bool from client showing booster status
		/// </summary>
		//TODO: Find out if this is a flag or an enum
		public byte Boosting { get; set; }

		/// <summary>
		/// </summary>
		//TODO: Pull this from data files (poo + pilot growth)
		public int Health { get; set; }

		/// <summary>
		/// This units max health
		/// Assigned at runtime via stat calculations
		/// </summary>
		public int MaxHealth { get { return 1000; } }

		/// <summary>
		/// The current weapon set this user is using
		/// </summary>
		private int _currentWeaponSet = 0;

		/// <summary>
		/// Gets the current weapon set equipped
		/// </summary>
		public int CurrentWeaponSet { get { return _currentWeaponSet; } }

		public bool Alive { get; set; }

		/// <summary>
		/// Stores the time value the client has sent us. This will be used to calculate cooldowns i think
		/// </summary>
		private uint _lastTick = 0;

		/// <summary>
		/// Gets the players last tick
		/// </summary>
		public uint GetLastTick { get { return _lastTick; } }

		/// <summary>
		/// Gets the weapon by arm based on the users current weapon set equipped
		/// </summary>
		/// <param name="arm">0 for left, 1 for right</param>
		/// <returns></returns>
		public Weapon GetWeaponByArm(int arm)
		{
			if (_currentWeaponSet == 0)
			{
				return arm == 0 ? WeaponSet1Left : WeaponSet1Right;
			}

			return arm == 0 ? WeaponSet2Left : WeaponSet2Right;
		}

		/// <summary>
		/// Called when we get an updated timestamp. Will trickle down tick calls with delta to stuff
		/// </summary>
		/// <param name="timeStamp"></param>
		//TODO: Should this be an abstract class for all items?
		public float UpdatePing(uint timeStamp)
		{
			// Calculate delta
			var delta = timeStamp - _lastTick;

			// Update timestamp
			_lastTick = timeStamp;

			// Return delta
			return delta;
		}
	}
}