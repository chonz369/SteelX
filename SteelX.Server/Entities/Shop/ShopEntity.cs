using System;
using System.Linq;
using SteelX.Shared;

namespace SteelX.Server
{
	/// <summary>
	/// Represents a single item contract and price
	/// </summary>
	public class ShopEntity
	{
		/// <summary>
		/// Shop Entry Id
		/// </summary>
		/// Primary Key
		public uint Id { get; private set; }
		
		/// <summary>
		/// Item Id for this Shop Id
		/// </summary>
		/// Foreign Key
		public uint[] ItemId { get { return TemplateString.Split(':').Select(t => Convert.ToUInt32(t)).ToArray(); } }
		
		/// <summary>
		/// Price in credits (In-Game Currency)
		/// </summary>
		public uint CreditPrice { get; set; }
		
		/// <summary>
		/// Price in coins (Premium Currency)
		/// </summary>
		public uint CoinPrice { get; set; }
		
		/// <summary>
		/// The contract type for this item
		/// </summary>
		public ContractTypes ContractType { get; set; }
		
		/// <summary>
		/// The product type this item is
		/// </summary>
		public ProductTypes ProductType { get; set; }
		
		/// <summary>
		/// The contract value for this item
		/// </summary>
		public uint ContractValue { get; set; }

		/// <summary>
		/// The computed templates in this item
		/// Always one, unless it is a unit set
		/// </summary>
		public string TemplateString { get; set; }
	}
}