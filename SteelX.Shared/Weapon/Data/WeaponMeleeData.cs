namespace SteelX.Shared
{
	public struct WeaponMeleeData : IWeaponData
	{
		public int Id { get; private set; }
		public bool NPCPart { get; private set; }
		public string Model { get; private set; }
		public string Name { get; private set; }
		public WeaponTypes WeaponType { get; private set; }
		public int Weight { get; private set; }
		public int Size { get; private set; }
		public int IFOSize { get; private set; }
		public int EnergyDrain { get; private set; }
		public bool TwoHanded { get; private set; }
		public int Damage { get; private set; }
		public float HitMin { get; private set; }
		public int Range { get; private set; }
		public int DefaultRange { get; private set; }
		public float DropRange { get; private set; }
		public float AttackFreezeTime { get; private set; }
		public float JumpAttackFreezeTime { get; private set; }
		public float DashDistance { get; private set; }
		public float JumpAttackDashDistance { get; private set; }
		public int LockOnAngle { get; private set; }
		public int JumpAttackAngle { get; private set; }
		public int SplashCount { get; private set; }
		public int SplashDamage { get; private set; }
		public int SplashAngle { get; private set; }
		public BlowTypes BlowType { get; private set; }
		public float FreezeTargetMove { get; private set; }
		public int BothDamage { get; private set; }
		public float BothAtkFreezTIme { get; private set; }
		public float BothDashDis { get; private set; }
		public float OverheatPoint { get; private set; }
		public float OverheatRecovery { get; private set; }
		public int ThrustProbability { get; private set; }
		public int MinThrustDistance { get; private set; }
		public int MaxThrustDistance { get; private set; }
		public int CriticalThrustProbability { get; private set; }
		public int MinCriticalThrustDistance { get; private set; }
		public int MaxCriticalThrustDistance { get; private set; }
		public float RepairTime { get; private set; }
		public int Grade { get; private set; }
		public string Description { get; private set; }
	}
}