namespace SteelX.Shared
{
	public class Mechanaughts
	{
		#region Variables
		public Arms Arm				{ get; private set; }
		public Legs Leg				{ get; private set; }
		public Cores Core			{ get; private set; }
		public Heads Head			{ get; private set; }
		public Boosters Booster		{ get; private set; }
		public Weapon[,] Weapons	{ get; private set; }
		public Skill[] Skills		{ get; private set; }

		//ToDo: Add Weapon Durability to Mech's?
		public int Durability		{ get { return Arm.Durability + Leg.Durability + Core.Durability + Head.Durability + Booster.Durability; } }
		public int Weight			{ get { return Arm.Weight + Leg.Weight + Core.Weight + Head.Weight + Booster.Weight; } }
		public int Size				{ get { return Arm.Size + Leg.Size + Core.Size + Head.Size + Booster.Size; } }
		public int MaxHP			{ get { return Arm.HP + Leg.HP + Core.HP + Head.HP + Booster.HP; } }
		public int HP				{ get; private set; }
		public int EN				{ get { return Core.EN; } }
		public int SP				{ get { return Head.SP; } }
		public int MPU				{ get { return Head.MPU; } }
		public int MoveSpeed		{ get { return Leg.BasicSpeed; } }
		public int EN_Recovery		{ get { return Arm.RecoveryEN + Leg.RecoveryEN + Core.RecoveryEN + Head.RecoveryEN + Booster.RecoveryEN; } }
		public int MinEN_Required	{ get { return Core.MinEN; } }
		public int[] OperatorStat
		{
			get
			{
				int[] PropertiesArray = new int[System.Enum.GetNames(typeof(OperatorStats)).Length];
				PropertiesArray[(byte)OperatorStats.HP]				= MaxHP;
				PropertiesArray[(byte)OperatorStats.EN]				= EN;
				PropertiesArray[(byte)OperatorStats.SP]				= SP;
				PropertiesArray[(byte)OperatorStats.MPU]				= MPU;
				PropertiesArray[(byte)OperatorStats.Size]			= Size;
				PropertiesArray[(byte)OperatorStats.Weight]			= Weight;
				//PropertiesArray[6] = (int)mechProperty.GetMoveSpeed(partWeight, weaponWeight);
				//PropertiesArray[7] = (int)mechProperty.GetDashSpeed(partWeight + weaponWeight);
				PropertiesArray[(byte)OperatorStats.ENOutputRate]	= ENOutputRate;
				PropertiesArray[(byte)OperatorStats.MinEN_Required]	= MinEN_Required;//MinENRequired;
				PropertiesArray[(byte)OperatorStats.DashENDrain]		= DashENDrain;
				PropertiesArray[(byte)OperatorStats.JumpENDrain]		= JumpENDrain;//GetJumpENDrain(partWeight + weaponWeight);
				//PropertiesArray[12] = mechProperty.GetDashAcceleration(partWeight + weaponWeight);
				//PropertiesArray[13] = mechProperty.GetDashDecelleration(partWeight + weaponWeight);
				PropertiesArray[(byte)OperatorStats.MaxHeat]			= MaxHeat;
				PropertiesArray[(byte)OperatorStats.CooldownRate]	= CooldownRate;
				PropertiesArray[(byte)OperatorStats.ScanRange]		= ScanRange;
				PropertiesArray[(byte)OperatorStats.Marksmanship]	= Marksmanship;

				return PropertiesArray;
			}
		}

		#region Arms
		public int MaxHeat			{ get { return Arm.MaxHeat; } }
		public int CooldownRate		{ get { return Arm.CooldownRate; } }
		public int Marksmanship		{ get { return Arm.Marksmanship; } }
		#endregion
		#region Legs
		public int BasicSpeed		{ get { return MoveSpeed; } }
		public int Capacity			{ get { return Leg.Capacity; } }
		public int Deceleration		{ get { return Leg.Deceleration; } }
		#endregion
		#region Core
		public int ENOutputRate		{ get { return Core.OutputRate; } }
		//public int MinENRequired { get; set; }
		public int EnergyDrain		{ get { return Arm.EnergyDrain + Leg.EnergyDrain + Core.EnergyDrain + Head.EnergyDrain + Booster.EnergyDrain; } }
		#endregion
		#region Head
		public int ScanRange		{ get { return Head.ScanRange; } }
		#endregion
		#region Booster
		public int DashOutput		{ get { return Booster.DashOutput; } }
		public int DashENDrain		{ get { return Booster.DashDrainEN; } }
		public int JumpENDrain		{ get { return Booster.JumpDrainEN; } }
		#endregion
		#endregion

		#region Constructor
		public Mechanaughts() 
		{
			Arm = new Arms(Parts.AGM001);
			Leg = new Legs(Parts.LGM001);
			Core = new Cores(Parts.CGM001);
			Head = new Heads(Parts.HGM001);
			Booster = new Boosters(Parts.PGM001);
		}
		public Mechanaughts(MechData mech) : this()
		{
			Arm = new Arms(mech.Arms.Id);
			Leg = new Legs(mech.Legs.Id);
			Core = new Cores(mech.Core.Id);
			Head = new Heads(mech.Head.Id);
			Booster = new Boosters(mech.Booster.Id);
		}
		public Mechanaughts LoadOut(MechWeapon weap, byte set = 1)
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

		public float GetJumpENDrain(int totalWeight)
		{
			return JumpENDrain + totalWeight / 160f;//TODO : improve this
		}

		public float GetDashSpeed(int totalWeight)
		{
			return DashOutput * 1.8f - totalWeight * 0.004f; //DashOutput * 1.8f : max speed  ;  0.004 weight coefficient
		}

		public float GetMoveSpeed(int partWeight, int weaponWeight)
		{
			int cal_capacity = (Capacity > 195000) ? 195000 : Capacity;

			double x1 = 0.0001064 * cal_capacity + 190.2552f, x2 = -0.0000024659 * cal_capacity + 0.69024f;
			//Debug.Log("part weight : "+partWeight + " weapon Weight : "+weaponWeight);
			//Debug.Log("basic speed : "+BasicSpeed + " coeff x1 : "+x1+" , x2 : "+x2);
			return (float)(BasicSpeed - (partWeight * x2 + weaponWeight) / x1);
		}

		public float GetDashAcceleration(int totalWeight)
		{
			return GetDashSpeed(totalWeight) / 100f - 1;
		}

		public float GetDashDecelleration(int totalWeight)
		{
			return Deceleration / 10000f - (totalWeight - Deceleration) / 20000f;
		}
		#endregion
	}
}