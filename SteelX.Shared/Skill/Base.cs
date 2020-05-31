namespace SteelX.Shared
{
	/// <summary>
	/// Use this class to create the base, 
	/// for all skills to be used later in game
	/// </summary>
	public abstract class Skill 
	{
		//int Cost
		//int CoolDown
		//Weapon[] Weapon
		public SteelX.Shared.Skills Id { get; private set; }
	}
}