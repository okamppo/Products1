using System;
using Xamarin.Forms;

namespace Products1
{
	public class NewSaleCell : ViewCell
	{
		public NewSaleCell ()
		{
			var saleDetailIDLabel = new Label {
				XAlign = TextAlignment.End,
				FontSize = 20,
				HorizontalOptions=LayoutOptions.Start
			};
			saleDetailIDLabel.SetBinding (Label.TextProperty, new Binding ("ProductID"));

			var productDescriptionLabel = new Label {
				XAlign = TextAlignment.Start,
				HorizontalOptions=LayoutOptions.FillAndExpand
			};
			productDescriptionLabel.SetBinding (Label.TextProperty, new Binding ("Description"));

			var productQuantityLabel = new Label {
				XAlign = TextAlignment.Center,
				HorizontalOptions=LayoutOptions.FillAndExpand,
			};
			productQuantityLabel.SetBinding (Label.TextProperty, new Binding ("Quantity", stringFormat: "Cantidad: {0}"));

			var productPriceLabel = new Label {
				XAlign = TextAlignment.End,
				HorizontalOptions=LayoutOptions.End,
				TextColor=Device.OnPlatform(Color.Navy,Color.White,Color.White)
			};
			productPriceLabel.SetBinding (Label.TextProperty, new Binding ("Price" , stringFormat: "Precio unidad: {0:C2}"));

			View = new StackLayout{ 
				Children={ saleDetailIDLabel, productDescriptionLabel, productQuantityLabel, productPriceLabel },
				Orientation=StackOrientation.Horizontal
			};
		}
	}
}

