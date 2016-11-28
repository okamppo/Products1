using System;

namespace Products1
{
	public class Product
	{
		public int ProductID {get; set;}	
		public string Description {get; set;}	
		public decimal Price {get; set;}	
		public string PriceFormated {get{ return string.Format ("{0:C2}", Price);}}	
		public DateTime LastBuy {get; set;}	
		public float Stock {get; set;}	
		public string Remarks {get; set;}	
		public int CategoryID {get; set;}	
		public int TaxID {get; set;}	

		public override string ToString ()
		{
			return string.Format ("[Product: ProductID={0}, Description={1}, Price={2}, LastBuy={3}, Stock={4}, Remarks={5}, CategoryID={6}, TaxID={7}]", ProductID, Description, Price, LastBuy, Stock, Remarks, CategoryID, TaxID);
		}
	}
}

