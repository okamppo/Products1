using System;
using System.Collections.Generic;

namespace Products1
{
	public class SaleView
	{
		public Customer Customer { get; set; }
		public Sale Sale { get; set; }
		public List<SaleDetail> SaleDetails { get; set; }
	}
}

