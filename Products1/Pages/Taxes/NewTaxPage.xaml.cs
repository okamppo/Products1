using System;
using System.Collections.Generic;

using Xamarin.Forms;
using System.Text;
using Newtonsoft.Json;
using System.Net.Http;

namespace Products1
{
	public partial class NewTaxPage : ContentPage
	{
		private DeviceUser deviceUser;

		public NewTaxPage (DeviceUser deviceUser)
		{
			InitializeComponent ();
			this.deviceUser = deviceUser;

			newButton.Clicked += newButton_Clicked;
		}
		private async void newButton_Clicked(object sender, EventArgs e)
		{
			if(string.IsNullOrEmpty(descriptionEntry.Text)){
				await DisplayAlert("Error","Debe ingresar una descripción","Aceptar");
				descriptionEntry.Focus ();
				return;
			}
			if (string.IsNullOrEmpty (taxPercentEntry.Text)) {
				await DisplayAlert ("Error", "Debe ingresar un porcentaje", "Aceptar");
				taxPercentEntry.Focus ();
				return;
			}
			waitActivityIndicator.IsRunning = true;

			var tax = new Tax {
				Description=descriptionEntry.Text,
				TaxPercent = float.Parse(taxPercentEntry.Text)
			};

			var jsonRequest = JsonConvert.SerializeObject (tax);
			var content = new StringContent (jsonRequest, Encoding.UTF8,"text/json");


			string result;
			try {
				HttpClient client = new HttpClient ();
				client.BaseAddress = new Uri ("http://zulu-software.com");
				string url = string.Format ("/Z-Market/api/TaxesAPI");
				var response = await client.PostAsync(url,content);
				result = response.Content.ReadAsStringAsync ().Result;
			} catch (Exception) {
				DisplayAlert ("Error", "No hay conexión, intente mas tarde", "Aceptar");			
				waitActivityIndicator.IsRunning = false;
				return;	
			}  		

			waitActivityIndicator.IsRunning = false;	
			await DisplayAlert ("Confirmación", "Impuesto Creado", "Aceptar");
			await Navigation.PushAsync (new TaxesPage (deviceUser)); 
		}
	}
}

