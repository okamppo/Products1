using System;
using Xamarin.Forms;

namespace Products1
{
	public class CategoryCell : ViewCell
	{
		public CategoryCell ()
		{
			var categoryIDLabel = new Label {
				XAlign = TextAlignment.End,
				FontSize = 20,
				HorizontalOptions=LayoutOptions.Start
			};
			categoryIDLabel.SetBinding (Label.TextProperty, new Binding ("CategoryID"));

			var descriptioncategoryLabel = new Label {
				XAlign = TextAlignment.Start,
				HorizontalOptions=LayoutOptions.FillAndExpand
			};
			descriptioncategoryLabel.SetBinding (Label.TextProperty, new Binding ("Description"));


			View = new StackLayout{ 
				Children={categoryIDLabel,descriptioncategoryLabel},
				Orientation=StackOrientation.Horizontal
			};
		}
	}
}

