using SteelX.Shared;

namespace SteelX.Shared
{
	/// <summary>
	/// Use this when storing modifications and attribute info to parts
	/// </summary>
	public struct PartMetaData
	{
		//public MechSlots Type	{ get; private set; }
		public Parts Id			{ get; private set; }
		public Parts Color		{ get; private set; }
	}
}