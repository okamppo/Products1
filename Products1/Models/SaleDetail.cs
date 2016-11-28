using System;

namespace Products1
{
	public class SaleDetail
	{
		public int SaleDetailID { get; set; }
		public int SaleID { get; set; }
		public int ProductID { get; set; }
		public string Description { get; set; }
		public decimal Price { get; set; }
		public float Quantity { get; set; }
	}
}

