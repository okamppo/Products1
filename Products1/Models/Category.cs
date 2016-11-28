using System;

namespace Products1
{
	public class Category
	{
		public int CategoryID {get; set;}	
		public string Description {get; set;}	

		public override string ToString ()
		{
			return string.Format ("[Category: CategoryID={0}, Description={1}", CategoryID, Description);
		}
	}
}

