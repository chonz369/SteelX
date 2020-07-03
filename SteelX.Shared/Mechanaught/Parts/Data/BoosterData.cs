namespace SteelX.Shared
{
	public struct BoosterData : IPartData
	{
		#region Variables
		public Parts Part { get; set; }
		public int Id { get; set; }
		public bool NPCPart { get; private set; }
		public string Model { get; private set; }
		public string Name { get; private set; }
		public int WeaponType { get; private set; }
		public int IFOType { get; private set; }
		public int NotifyCount { get; private set; }
		public int BoneCount { get; private set; }
		public int Weight { get; private set; }
		public int HitPoint { get; private set; }
		/// <summary>
		/// Rate of speed that is increased when dashing.
		/// </summary>
		public int BoosterPower { get; private set; }
		/// <summary>
		/// How fast Energy is drained when dashing.
		/// </summary>
		public int BoosterEnDrain { get; private set; }
		public int Size { get; private set; }
		public int EnergyDrain { get; private set; }
		/// <summary>
		/// How fast Energy is drained when jumping.
		/// </summary>
		public int BoosterJumpEnDrain { get; private set; }
		public float BoostJumpVelocity { get; private set; }
		public float BoostJumpMoveVelocity { get; private set; }
		public float RepairTime { get; private set; }
		public int IFOSize { get; private set; }
		public bool StopAttack { get; private set; }
		public float AttackFreezeTime { get; private set; }
		public int PrjSpeed { get; private set; }
		public int IFORadius { get; private set; }
		public int Grade { get; private set; }
		public string Description { get; private set; }
		#endregion
	}
}