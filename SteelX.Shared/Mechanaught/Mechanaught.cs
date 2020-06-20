namespace SteelX.Shared
{
	public class Mechanaught
	{
		#region Variables
		public Parts Arm			{ get; private set; }
		public Parts Leg			{ get; private set; }
		public Parts Core			{ get; private set; }
		public Parts Head			{ get; private set; }
		public Parts Booster		{ get; private set; }
		public MechData Mech		{ get; private set; }
		public Weapon[,] Weapons	{ get; private set; }
		public Skill[] Skills		{ get; private set; }

		#region Stats
		//ToDo: Add Weapon Durability to Mech's?
		public int Durability		{ get { return Application.PartsData[Arm].Durability + Application.PartsData[Leg].Durability + Application.PartsData[Core].Durability + Application.PartsData[Head].Durability + Application.PartsData[Booster].Durability; } }
		public int Weight			{ get { return Application.PartsData[Arm].Weight + Application.PartsData[Leg].Weight + Application.PartsData[Core].Weight + Application.PartsData[Head].Weight + Application.PartsData[Booster].Weight; } }
		public int Size				{ get { return Application.PartsData[Arm].Size + Application.PartsData[Leg].Size + Application.PartsData[Core].Size + Application.PartsData[Head].Size + Application.PartsData[Booster].Size; } }
		public int HP				{ get { return Application.PartsData[Arm].HP + Application.PartsData[Leg].HP + Application.PartsData[Core].HP + Application.PartsData[Head].HP + Application.PartsData[Booster].HP; } }
		public int EN				{ get { return Application.CoresData[Core].EN; } }
		public int SP				{ get { return Application.HeadsData[Head].SP; } }
		public int MPU				{ get { return Application.HeadsData[Head].MPU; } }
		public int MoveSpeed		{ get { return Application.LegsData[Leg].BasicSpeed; } }
		public int EN_Recovery		{ get { return Application.PartsData[Arm].RecoveryEN + Application.PartsData[Leg].RecoveryEN + Application.PartsData[Core].RecoveryEN + Application.PartsData[Head].RecoveryEN + Application.PartsData[Booster].RecoveryEN; } }
		public int MinEN_Required	{ get { return Application.CoresData[Core].MinEN; } }
		public int[] OperatorStats
		{
			get
			{
				int[] PropertiesArray = new int[System.Enum.GetNames(typeof(OperatorStats)).Length];
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
		#region Arms
		public int MaxHeat			{ get { return Application.ArmsData[Arm].MaxHeat; } }
		public int CooldownRate		{ get { return Application.ArmsData[Arm].CooldownRate; } }
		public int Marksmanship		{ get { return Application.ArmsData[Arm].Marksmanship; } }
		#endregion
		#region Legs
		public int BasicSpeed		{ get { return MoveSpeed; } }
		public int Capacity			{ get { return Application.LegsData[Leg].Capacity; } }
		public int Deceleration		{ get { return Application.LegsData[Leg].Deceleration; } }
		#endregion
		#region Core
		public int ENOutputRate		{ get { return Application.CoresData[Core].OutputRate; } }
		//public int MinENRequired { get; set; }
		public int EnergyDrain		{ get { return Application.PartsData[Arm].EnergyDrain + Application.PartsData[Leg].EnergyDrain + Application.PartsData[Core].EnergyDrain + Application.PartsData[Head].EnergyDrain + Application.PartsData[Booster].EnergyDrain; } }
		#endregion
		#region Head
		public int ScanRange		{ get { return Application.HeadsData[Head].ScanRange; } }
		#endregion
		#region Booster
		public int DashOutput		{ get { return Application.BoostersData[Booster].DashOutput; } }
		public int DashENDrain		{ get { return Application.BoostersData[Booster].DashDrainEN; } }
		public int JumpENDrain		{ get { return Application.BoostersData[Booster].JumpDrainEN; } }
		#endregion
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
		public Mechanaught(MechData mech) : this()
		{
			Mech = mech;
		}
		//public Mechanaught(Mech mech) : this()
		//{
		//	Arm = new Arms(mech.Arms);
		//	Leg = new Legs(mech.Legs);
		//	Core = new Cores(mech.Core);
		//	Head = new Heads(mech.Head);
		//	Booster = new Boosters(mech.Booster);
		//}
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

		#region Sample Code from Alpha
		//public int HP, EN, SP, MPU;
		//public int Size, Weight;
		public int VerticalBoostSpeed { get; set; }
		private float DashAcceleration { get; set; }
		private float DashDecelleration { get; set; }

		public float GetJumpENDrain()
		{
			return JumpENDrain + Weight / 160f;//TODO : improve this
		}

		public float GetDashSpeed()
		{
			return DashOutput * 1.8f - Weight * 0.004f; //DashOutput * 1.8f : max speed  ;  0.004 weight coefficient
		}

		public float GetMoveSpeed(int partWeight, int weaponWeight)
		{
			int cal_capacity = (Capacity > 195000) ? 195000 : Capacity;

			double x1 = 0.0001064 * cal_capacity + 190.2552f, x2 = -0.0000024659 * cal_capacity + 0.69024f;
			//Debug.Log("part weight : "+partWeight + " weapon Weight : "+weaponWeight);
			//Debug.Log("basic speed : "+BasicSpeed + " coeff x1 : "+x1+" , x2 : "+x2);
			return (float)(BasicSpeed - (partWeight * x2 + weaponWeight) / x1);
		}

		public float GetDashAcceleration()
		{
			return GetDashSpeed() / 100f - 1;
		}

		public float GetDashDecelleration()
		{
			return Deceleration / 10000f - (Weight - Deceleration) / 20000f;
		}
		#endregion
	}
}