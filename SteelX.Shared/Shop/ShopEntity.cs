namespace SteelX.Shared
{
	public struct ShopEntity
	{
		public int Id						{ get; private set; }
		public int CreditPrice 				{ get; private set; }
		public int CoinPrice 				{ get; private set; }
		public string ItemNameCode 			{ get; private set; }
		public string ItemDescCode 			{ get; private set; }
		public ProductTypes ProductType 	{ get; private set; }
		public ContractTypes ContractType 	{ get; private set; }
		public int ContractValue   			{ get; private set; }
		public string TemplateString		{ get; private set; }
	}
}