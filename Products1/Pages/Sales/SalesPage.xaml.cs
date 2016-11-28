using System;
using System.Collections.Generic;

using Xamarin.Forms;
using System.Net.Http;
using Newtonsoft.Json;
using System.Linq;

namespace Products1
{
	public partial class SalesPage : ContentPage
	{
		private List<SaleView> salesView;
		private DeviceUser deviceUser;

		public SalesPage (DeviceUser deviceUser)
		{
			InitializeComponent ();
			this.deviceUser = deviceUser;
			this.titleLabel.Text = this.deviceUser.NickName;
			salesListView.ItemTemplate = new DataTemplate (typeof(SaleCell));
			this.LoadSales ();

			newButton.Clicked += newButton_Clicked;
			findButton.Clicked += findButton_Clicked;
			salesListView.ItemTapped += salesListView_ItemTapped;
		}

		private async void findButton_Clicked(object sender, EventArgs e)
		{
			await Navigation.PushAsync (new SearchSalePage(this.deviceUser, this.salesView));
		}

		private async void salesListView_ItemTapped(object sender, ItemTappedEventArgs e)
		{
			var sale = e.Item as SaleView;
			await Navigation.PushAsync (new SaleDetailsPage(this.deviceUser, sale));	
		}

		private async void newButton_Clicked(object sender, EventArgs e)
		{
			await Navigation.PushAsync (new NewSalePage(this.deviceUser, null));
		}

		private async void LoadSales()
		{
			waitActivityIndicator.IsRunning = true;
			string result;
			try 
			{
				HttpClient client = new HttpClient ();
				client.BaseAddress = new Uri("http://zulu-software.com");
				string url = string.Format("/Z-Market/api/SalesAPI/");
				var response = await client.GetAsync(url);
				result = response.Content.ReadAsStringAsync().Result;

				if (result != null)
				{
					List<Sale> sales = new List<Sale>();
					sales= JsonConvert.DeserializeObject<List<Sale>>(result);

					HttpClient client2 = new HttpClient ();
					client2.BaseAddress = new Uri("http://zulu-software.com");
					string url2 = string.Format("/Z-Market/api/SaleDetailsAPI/");
					var response2 = await client2.GetAsync(url2);
					string result2 = response2.Content.ReadAsStringAsync().Result;

					List<SaleDetail> salesDetails = new List<SaleDetail>();
					salesDetails = JsonConvert.DeserializeObject<List<SaleDetail>>(result2);				

					salesView = new List<SaleView>();
					foreach(var sale in sales)
					{
						List<SaleDetail> details = salesDetails.Where(s => s.SaleID == sale.SaleID).ToList();

						HttpClient client3 = new HttpClient ();
						client3.BaseAddress = new Uri("http://zulu-software.com");
						string url3 = string.Format("/Z-Market/api/CustomersAPI/{0}", sale.CustomerID);
						var response3 = await client3.GetAsync(url3);
						string result3 = response3.Content.ReadAsStringAsync().Result;

						Customer customer = new Customer();
						customer = JsonConvert.DeserializeObject<Customer>(result3);

						SaleView saleView = new SaleView {
							Sale = sale,
							Customer = customer,
							SaleDetails = details
						};

						salesView.Add(saleView);
					}
				}
			} 
			catch (Exception) 
			{
				DisplayAlert ("Error", "No hay conexión, intente mas tarde", "Aceptar").Wait();			
				waitActivityIndicator.IsRunning = false;
				return;	
			}  		

			waitActivityIndicator.IsRunning = false;	
			salesListView.ItemsSource = salesView;
		}
	}
}

