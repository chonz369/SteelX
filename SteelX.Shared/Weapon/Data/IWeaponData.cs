namespace SteelX.Shared
{
	public interface IWeaponData : IPartData
	{
		/// <summary>
		/// How many points added to overheat 
		/// </summary>
		float OverheatPoint { get; }
		float OverheatRecovery { get; }
		bool TwoHanded { get; }
		WeaponTypes WeaponType { get; }
	}
}