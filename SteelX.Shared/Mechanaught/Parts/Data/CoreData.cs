namespace SteelX.Shared
{
	public struct CoreData : IPartData
	{
		#region Variables
		public Parts Part { get; private set; }
		public int Id { get; private set; }
		public bool NPCPart { get; private set; }
		public string Model { get; private set; }
		public string Name { get; private set; }
		public int Weight { get; private set; }
		public int HitPoint { get; private set; }
		public int Size { get; private set; }
		public int EnergyDrain { get; private set; }
		/// <summary>
		/// EN sets the minimum EN of the Mechanaught
		/// </summary>
		public int Capacity { get; private set; }
		/// <summary>
		/// EN Output Rate sets the rate of EN regeneration of a mechanaught
		/// </summary>
		public int Generate { get; private set; }
		/// <summary>
		/// Minimum EN Required
		/// </summary>
		public int MinSupply { get; private set; }
		public int IFOSize { get; private set; }
		public int BoneType { get; private set; }
		public float RepairTime { get; private set; }
		public int Grade { get; private set; }
		public string Description { get; private set; }
		#endregion
	}
}