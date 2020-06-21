namespace SteelX.Shared
{
	public class Player //ToDo: Rename to "User"?
	{
		//Inventory List<> Parts
		//Mechs List<> Builds
		//Mechs Array[SlotsAvailable] LoadOuts
		//public int Credits { get; set; }
		//int RepairPoints
		//byte LoadoutSlots
		//Mech Mech //Active Mech
		//Pilot Pilot //Active Pilot
		//int[] Stats/Experience Points //Feed From Server, may not need it in class
		public MechData ActiveMech { get; private set; }
		public System.Guid Uid { get; private set; }
		public string Username { get; private set; }
		public string PilotName { get; private set; }
		public int Level { get; private set; }
		public int Rank { get; private set; }
		public int Credits { get; private set; }
		//parts[] inventoryid

		//ToDo: Move to Global Class
		//internal Mechanaughts LoadFromServer(System.Guid HashId)
		//{
		//	//GetPart(Id);
		//	//SetColor(int, datetime expire)
		//	//SetDurability(int)
		//	throw new System.Exception("Part ID doesnt exist in the database. Please check Arms constructor.");
		//}

		#region Nested Classes
		#endregion
	}
}