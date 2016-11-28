using System;
using Xamarin.Forms;

namespace Products1
{
	public class SaleCell : ViewCell
	{
		public SaleCell ()
		{
			var saleIDLabel = new Label {
				XAlign = TextAlignment.End,
				FontSize = 20,
				HorizontalOptions=LayoutOptions.Start
			};
			saleIDLabel.SetBinding (Label.TextProperty, new Binding ("Sale.SaleID"));

			var customerNameLabel = new Label {
				XAlign = TextAlignment.Start,
				HorizontalOptions=LayoutOptions.FillAndExpand
			};
			customerNameLabel.SetBinding (Label.TextProperty, new Binding ("Customer.FullName", stringFormat: "Cliente: {0}"));

			var dateSaleLabel = new Label {
				XAlign = TextAlignment.End,
				HorizontalOptions=LayoutOptions.End,
				TextColor=Device.OnPlatform(Color.Navy,Color.Black,Color.White)
			};
			dateSaleLabel.SetBinding (Label.TextProperty, new Binding ("Sale.DateSale", stringFormat: "{0:d}"));

			View = new StackLayout{ 
				Children={ saleIDLabel, customerNameLabel, dateSaleLabel },
				Orientation=StackOrientation.Horizontal
			};
		}
	}
}

