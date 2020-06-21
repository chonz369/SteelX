using System.Collections.Generic;

namespace SteelX.Shared
{
	public class Imventory
	{
		//Inventory List<> Parts
		//Mechs List<> Builds
		//Mechs Array[SlotsAvailable] LoadOuts
		/// <summary>
		/// Player Mechs in Hanger, 
		/// with Key as Mech Name
		/// and Value as Mech Parts
		/// </summary>
		public Dictionary<string,MechData> Hanger { get; private set; }
		//ToDo: Need an index of item in inventory and what the player is using it on
	}
}