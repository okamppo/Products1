using System;

namespace Products1
{
	public class Tax
	{
		public int TaxID {get; set;}	
		public string Description {get; set;}	
		public float TaxPercent {get; set;}	

		public override string ToString ()
		{
			return string.Format ("[Tax: TaxID={0}, Description={1},TaxPercent={2}", TaxID, Description,TaxPercent);
		}
	}
}

