using System;
using System.Collections.Generic;

using Xamarin.Forms;
using System.Net.Http;
using Newtonsoft.Json;

namespace Products1
{
	public partial class SearchProductPage : ContentPage
	{
		private DeviceUser deviceUser;
		public SearchProductPage (DeviceUser deviceUser)
		{
			InitializeComponent ();
			this.deviceUser = deviceUser;
			searchButton.Clicked += searchButton_Clicked;
				
		}
		private async void searchButton_Clicked(object sender,EventArgs e){
			if(string.IsNullOrEmpty(productIDEntry.Text))
			{
				DisplayAlert ("Error","Debe ingresar un ID de producto","Aceptar").Wait();
				productIDEntry.Focus ();
				return;
			}

			waitActivityIndicator.IsRunning = true;
			searchButton.IsEnabled = false;
			string result;
			try 
			{
				HttpClient client = new HttpClient ();
				client.BaseAddress = new Uri ("http://zulu-software.com");
				string url = string.Format ("/Z-Market/api/ProductsAPI/{0}",productIDEntry.Text);
				var response = await client.GetAsync(url);
				result = response.Content.ReadAsStringAsync ().Result;
			} 
			catch (Exception) 
			{
				DisplayAlert ("Error", "No hay conexión, intente mas tarde", "Aceptar").Wait();			
				waitActivityIndicator.IsRunning = false;
				searchButton.IsEnabled = true;
				return;	
			}  		

			searchButton.IsEnabled = true;
			waitActivityIndicator.IsRunning = false;

			if(string.IsNullOrEmpty(result))
			{
				DisplayAlert ("Error", string.Format("El producto: {0} no existe",productIDEntry.Text),"Aceptar").Wait();			
				productIDEntry.Text = string.Empty;
				productIDEntry.Focus ();
				return;
			}

			var product = JsonConvert.DeserializeObject<Product> (result);
			await Navigation.PushAsync (new EditDeleteProductPage (this.deviceUser, product));

		}
	}
}

