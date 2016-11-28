using System;
using Xamarin.Forms;

namespace Products1
{
	public class ProductCell: ViewCell
	{
		public ProductCell ()
		{
			var productIDLabel = new Label {
				XAlign = TextAlignment.End,
				FontSize = 20,
				HorizontalOptions=LayoutOptions.Start
			};
			productIDLabel.SetBinding (Label.TextProperty, new Binding ("ProductID"));

			var descriptionLabel = new Label {
				XAlign = TextAlignment.Start,
				HorizontalOptions=LayoutOptions.FillAndExpand
			};
			descriptionLabel.SetBinding (Label.TextProperty, new Binding ("Description"));

			var priceLabel = new Label {
				XAlign = TextAlignment.End,
				HorizontalOptions=LayoutOptions.End,
				TextColor=Device.OnPlatform(Color.Navy,Color.White,Color.White)
			};
			priceLabel.SetBinding (Label.TextProperty, new Binding ("PriceFormated"));

			View = new StackLayout{ 
				Children={productIDLabel,descriptionLabel,priceLabel},
				Orientation=StackOrientation.Horizontal
			};
		}
	}
}

