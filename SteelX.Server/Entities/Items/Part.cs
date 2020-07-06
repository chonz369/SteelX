using SteelX.Shared;

namespace SteelX.Server
{
	/// <summary>
	/// Base class for a part
	/// </summary>
	public class Part
	{
		/// <summary>
		/// Unique part Id for this part.
		/// </summary>
		public uint Id { get; private set; }
		
		/// <summary>
		/// Unique template Id for this part
		/// </summary>
		public uint TemplateId { get; private set; }
		
		/// <summary>
		/// Unique template Id for this part
		/// </summary>
		public IPartData PartData { get; }

		/// <summary>
		/// Durability or amount of time left on item before expiration
		/// </summary>
		public ushort Parameters { get; private set; }

		/// <summary>
		/// The color of this part.
		/// </summary>
		public Colors Color { get; private set; }

		/// <summary>
		/// What type of mech part this item part belongs to
		/// </summary>
		/// Whether the item is marked to expire based on an expiration date,
		/// or a durability usage limit.
		public ProductTypes Type { get; private set; }
	}
}