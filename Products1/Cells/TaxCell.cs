using System;
using Xamarin.Forms;

namespace Products1
{
	public class TaxCell : ViewCell
	{
		public TaxCell ()
		{
			var taxIDLabel = new Label {
				XAlign = TextAlignment.End,
				FontSize = 20,
				HorizontalOptions=LayoutOptions.Start
			};
			taxIDLabel.SetBinding (Label.TextProperty, new Binding ("TaxID"));

			var descriptiontaxLabel = new Label {
				XAlign = TextAlignment.Start,
				HorizontalOptions=LayoutOptions.FillAndExpand
			};
			descriptiontaxLabel.SetBinding (Label.TextProperty, new Binding ("Description"));


			View = new StackLayout{ 
				Children={taxIDLabel,descriptiontaxLabel},
				Orientation=StackOrientation.Horizontal
			};
		}
	}
}

