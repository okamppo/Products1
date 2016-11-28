using System;
using System.Collections.Generic;

using Xamarin.Forms;

namespace Products1
{
	public partial class SaleDetailsPage : ContentPage
	{
		private DeviceUser deviceUser;
		private SaleView saleView;

		#region Propiedades
		public Customer Customer { get; set;}
		public Sale Sale { get; set;}
		public decimal Total { get; set;}
		#endregion

		public SaleDetailsPage (DeviceUser deviceUser, SaleView saleView)
		{
			InitializeComponent ();
			this.deviceUser = deviceUser;
			this.saleView = saleView;
			this.Customer = this.saleView.Customer;
			this.Sale = this.saleView.Sale;

			this.CalculateTotal ();
			this.SetBindings ();
			this.detailsListView.ItemTemplate = new DataTemplate (typeof(SaleDetailCell));
			this.detailsListView.ItemsSource = this.saleView.SaleDetails;
			backButton.Clicked += backButton_Clicked;
		}

		private async void backButton_Clicked(object sender, EventArgs e)
		{
			await Navigation.PushAsync (new SalesPage(deviceUser));	
		}

		private void CalculateTotal()
		{
			foreach(var detail in saleView.SaleDetails)
			{
				this.Total += detail.Price * Convert.ToDecimal(detail.Quantity);
			}
		}

		private void SetBindings()
		{
			saleIDEntry.SetBinding (Entry.TextProperty, new Binding ("SaleID", source: Sale));
			customerFullNameEntry.SetBinding (Entry.TextProperty, new Binding ("FullName", source: Customer));
			customerDocumentEntry.SetBinding (Entry.TextProperty, new Binding ("Document", source: Customer));
			saleDateEntry.SetBinding (Entry.TextProperty, new Binding ("DateSale", stringFormat: "{0:d}", source: Sale));
			saleTotalEntry.Text = string.Format("{0:C2}",Total);
		}
	}
}

