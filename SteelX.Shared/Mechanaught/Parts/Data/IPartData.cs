namespace SteelX.Shared
{
	public interface IPartData
	{
		int Id { get; }
		bool NPCPart { get; }
		string Name { get; }
		int EnergyDrain { get; }
		int IFOSize { get; }
		string Model { get; }
		float RepairTime { get; }
		int Size { get; }
		int Weight { get; }
		int Grade { get; }
		string Description { get; }
	}
}