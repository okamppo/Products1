using System;
using System.Collections.Generic;

using Xamarin.Forms;
using System.Net.Http;
using Newtonsoft.Json;

namespace Products1
{
	public partial class TaxesPage : ContentPage
	{
		private List<Tax> taxes;
		private DeviceUser deviceUser;
		public TaxesPage (DeviceUser deviceUser)
		{
			InitializeComponent ();
			this.deviceUser = deviceUser;
			taxesListView.ItemTemplate = new DataTemplate (typeof(TaxCell));
			this.LoadTaxes ();

			newTaxButton.Clicked += newTaxButton_Clicked;
			findTaxButton.Clicked += findTaxButton_Clicked;
			taxesListView.ItemTapped += taxesListView_ItemTapped;
		}
		private async void newTaxButton_Clicked(object sender, EventArgs e){
			await Navigation.PushAsync (new NewTaxPage (this.deviceUser)); 
		}
		private async void findTaxButton_Clicked(object sender, EventArgs e){
			await Navigation.PushAsync (new SearchTaxPage(this.deviceUser));
		}
		private async void taxesListView_ItemTapped(object sender, ItemTappedEventArgs e){
			var tax = e.Item as Tax;
			await Navigation.PushAsync (new EditDeleteTaxPage(this.deviceUser,tax)); 
		}


		private async void LoadTaxes()
		{
			waitActivityIndicator.IsRunning = true;
			string result;
			try {
				HttpClient client = new HttpClient ();
				client.BaseAddress = new Uri ("http://zulu-software.com");
				string url = string.Format ("/Z-Market/api/TaxesAPI");
				var response = await client.GetAsync (url);
				result = response.Content.ReadAsStringAsync ().Result;
			} catch (Exception) {
				DisplayAlert ("Error", "No hay conexión, intente mas tarde", "Aceptar");			
				waitActivityIndicator.IsRunning = false;
				return;	
			}  		

			waitActivityIndicator.IsRunning = false;	
			taxes= JsonConvert.DeserializeObject<List<Tax>>(result);
			taxesListView.ItemsSource = taxes;
		}
	}
}

