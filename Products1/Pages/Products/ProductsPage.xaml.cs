using System;
using System.Collections.Generic;

using Xamarin.Forms;
using System.Net.Http;
using Newtonsoft.Json;

namespace Products1
{
	public partial class ProductsPage : ContentPage
	{
		private List<Product> products;
		private DeviceUser deviceUser;
		public ProductsPage (DeviceUser deviceUser)
		{
			InitializeComponent ();
			this.deviceUser = deviceUser;
			titleLabel.Text = string.Format ("Bienvenid@ {0}{1}", this.deviceUser.FirstName, this.deviceUser.LastName);
			productsListView.ItemTemplate = new DataTemplate (typeof(ProductCell));
			this.LoadProducts ();

			newButton.Clicked += newButton_Clicked;
			findButton.Clicked += findButton_Clicked;
			productsListView.ItemTapped += productsListView_ItemTapped;
		}

		private async void findButton_Clicked(object sender, EventArgs e)
		{
			await Navigation.PushAsync (new SearchProductPage(this.deviceUser));
		}
			
		private async void productsListView_ItemTapped(object sender, ItemTappedEventArgs e)
		{
			var product = e.Item as Product;
			await Navigation.PushAsync (new EditDeleteProductPage(this.deviceUser,product));	
		}

		private async void newButton_Clicked(object sender, EventArgs e)
		{
			await Navigation.PushAsync (new NewProductPage (this.deviceUser));
		}

		private async void LoadProducts()
		{
			waitActivityIndicator.IsRunning = true;
			string result;
			try 
			{
                HttpClient client = new HttpClient();
                client.BaseAddress = new Uri("http://zulu-software.com");
                string url = string.Format("/Z-Market/api/ProductsAPI");
                var response = await client.GetAsync(url);
                result = response.Content.ReadAsStringAsync().Result;
            } 
			catch (Exception) 
			{
				DisplayAlert ("Error", "No hay conexión, intente mas tarde", "Aceptar").Wait();			
				waitActivityIndicator.IsRunning = false;
				return;	
			}  		

			waitActivityIndicator.IsRunning = false;	
			products= JsonConvert.DeserializeObject<List<Product>>(result);
			productsListView.ItemsSource=products;
		}
	}
}

