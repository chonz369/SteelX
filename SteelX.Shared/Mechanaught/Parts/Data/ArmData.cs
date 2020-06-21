namespace SteelX.Shared
{
	public struct ArmData
	{
		#region Variables
		/// <summary>
		/// Mesh Model Used
		/// </summary>
		public Parts Part { get; private set; }
		public int Id { get; private set; }
		/// <summary>
		/// Name of the Model's Prefab
		/// </summary>
		public string Model { get; private set; }
		/// <summary>
		/// 
		/// </summary>
		public int MaxHeat { get; private set; }
		/// <summary>
		/// 
		/// </summary>
		public int CooldownRate { get; private set; }
		/// <summary>
		/// 
		/// </summary>
		public int Marksmanship { get; private set; }
		/// <summary>
		/// If this part is used for Non-Playable Characters
		/// </summary>
		public bool NPCPart { get; private set; }
		/// <summary>
		/// 
		/// </summary>
		public int Size { get; private set; }
		/// <summary>
		/// 
		/// </summary>
		public int Weight { get; private set; }
		/// <summary>
		/// Health Points for this part 
		/// </summary>
		public int HitPoint { get; private set; }
		/// <summary>
		/// 
		/// </summary>
		public int EnergyDrain { get; private set; }
		/// <summary>
		/// Durability?
		/// </summary>
		public int Endurance { get; private set; }
		/// <summary>
		/// 
		/// </summary>
		public int IFOSize { get; private set; }
		/// <summary>
		/// 
		/// </summary>
		public int Aim { get; private set; }
		/// <summary>
		/// 
		/// </summary>
		public float RepairTime { get; private set; }
		/// <summary>
		/// Used for localization
		/// </summary>
		public string Description { get; private set; }
		/// <summary>
		/// Used for localization
		/// </summary>
		public string Name { get; private set; }
		/// <summary>
		/// Rank Required in order to utilize this part
		/// </summary>
		public int Grade { get; private set; }
		#endregion
	}
}