namespace SteelX.Shared
{
	public interface IWeaponData
	{
		string Description { get; }
		int EnergyDrain { get; }
		int Grade { get; }
		int Id { get; }
		int IFOSize { get; }
		string Model { get; }
		string Name { get; }
		bool NPCPart { get; }
		float RepairTime { get; }
		int Size { get; }
		bool TwoHanded { get; }
		WeaponTypes WeaponType { get; }
		int Weight { get; }
	}
}