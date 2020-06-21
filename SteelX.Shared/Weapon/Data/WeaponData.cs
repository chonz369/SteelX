namespace SteelX.Shared
{
	//ToDo: similiar to NPCData, but instead with weapons...
	//eventitem Number xxxxxxx=  x[PartsType=1~9]+x[PC=PartsType/NPC=0/GM=1/Event=2]+xxxxx[Vari1~99999]
	public struct WeaponData
	{
		public int Id { get; private set; }
		public bool NPCPart { get; private set; }
		public string Model { get; private set; }
		/// <summary>
		/// Used for localization
		/// </summary>
		public string Name { get; private set; }
		public int WeaponType { get; private set; }
		public int IFOType { get; private set; }
		public int Weight { get; private set; }
		public int Size { get; private set; }
		public int IFOSize { get; private set; }
		public int EnergyDrain { get; private set; }
		public int TwoHanded { get; private set; }
		public int Damage { get; private set; }
		public float ReloadTime { get; private set; }
		public float HitMin { get; private set; }
		public int AmmoMax { get; private set; }
		public int StackNum { get; private set; }
		public int MinRange { get; private set; }
		public int Range { get; private set; }
		public int DefaultRange { get; private set; }
		public float DropRange { get; private set; }
		public bool StopAttack { get; private set; }
		public float AttackFreezeTime { get; private set; }
		public float RecoilTime { get; private set; }
		public float RecoilDistance { get; private set; }
		public float OverheatPoint { get; private set; }
		public float OverheatRecovery { get; private set; }
		public int HealEnergy { get; private set; }
		public int LockOnAngle { get; private set; }
		public int IFORadius { get; private set; }
		public int SplashRadius { get; private set; }
		public int SplashDamage { get; private set; }
		public int SplashCount { get; private set; }
		public int PrjSpeed { get; private set; }
		public int BlowType { get; private set; }
		public float FreezeTargetMove { get; private set; }
		public float RepairTime { get; private set; }
		/// <summary>
		/// Rank Required in order to utilize this part
		/// </summary>
		public int Grade { get; private set; }
		/// <summary>
		/// Used for localization
		/// </summary>
		public string Description { get; private set; }
	}
}