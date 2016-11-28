using System;
using System.Collections.Generic;

using Xamarin.Forms;
using System.Net.Http;
using Newtonsoft.Json;

namespace Products1
{
	public partial class SearchTaxPage : ContentPage
	{
		private DeviceUser deviceUser;
		public SearchTaxPage (DeviceUser deviceUser)
		{
			InitializeComponent ();
			this.deviceUser = deviceUser;
			searchButton.Clicked += searchButton_Clicked;
		}
		private async void searchButton_Clicked(object sender,EventArgs e){
			if(string.IsNullOrEmpty(taxIDEntry.Text)){
				DisplayAlert ("Error","Debe ingresar un ID de impuesto","Aceptar");
				taxIDEntry.Focus ();
				return;
			}

			waitActivityIndicator.IsRunning = true;
			searchButton.IsEnabled = false;
			string result;
			try {
				HttpClient client = new HttpClient ();
				client.BaseAddress = new Uri ("http://zulu-software.com");
				string url = string.Format ("/Z-Market/api/TaxesAPI/{0}",taxIDEntry.Text);
				var response = await client.GetAsync(url);
				result = response.Content.ReadAsStringAsync ().Result;
			} catch (Exception) {
				DisplayAlert ("Error", "No hay conexión, intente mas tarde", "Aceptar");			
				waitActivityIndicator.IsRunning = false;
				searchButton.IsEnabled = true;
				return;	
			}  		

			searchButton.IsEnabled = true;
			waitActivityIndicator.IsRunning = false;

			if(string.IsNullOrEmpty(result)){
				DisplayAlert ("Error", string.Format("El Impuesto: {0} no existe",taxIDEntry.Text),"Aceptar");			
				taxIDEntry.Text = string.Empty;
				taxIDEntry.Focus ();
				return;
			}

			var tax = JsonConvert.DeserializeObject<Tax> (result);
			await Navigation.PushAsync (new EditDeleteTaxPage (this.deviceUser, tax));
		}
	}
}

