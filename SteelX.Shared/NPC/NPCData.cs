namespace SteelX.Shared
{
	//ToDo: Maybe use this to replace MechData or PartData class?
	public struct NPCData
	{
		//public int Id				{ get; } //return opposite of constructor
		public MechSlots PartType	{ get; private set; }
		public bool NPCPart			{ get; private set; }
		public int NPCLevel			{ get; private set; }
		public NPCTypes NPCType		{ get; private set; }
		public int NPCVariable		{ get; private set; }

		public NPCData (int id)
		{
			//if(id >= 1000000 && id <= 8000000)
			//{
				//npcdata Number xxxxxxx= x[PartsType = 1~9]+x[PC = PartsType / NPC = 0]+x[NpcLevel = 1~9]+xx[NpcType = 1~99]+xx[NpcVari = 1~99]
				int[] data = new int[5];
				#region variable
				data[4] = id % 10;
				id /= 10;
				data[4] = data[4] + (id % 10);
				NPCVariable = data[4];
				id /= 10;
				#endregion
				#region npc type
				data[3] = id % 10;
				id /= 10;
				data[3] = data[3] + (id % 10);
				NPCType = (NPCTypes)data[3];
				id /= 10;
				#endregion
				#region level
				data[2] = id % 10;
				NPCLevel = data[2];
				id /= 10;
				#endregion
				#region npc part
				data[1] = id % 10;
				NPCPart = data[1] == 1; //0 = player, 1 = npc, 2 = GM
				id /= 10;
				#endregion
				#region part type
				data[0] = id % 10;
				PartType = (MechSlots)data[0];
				#endregion
			//}
			//else { } //else return default values
		}
	}
}