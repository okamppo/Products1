using System;
using System.Collections.Generic;

using Xamarin.Forms;
using Newtonsoft.Json;
using System.Net.Http;
using System.Text;

namespace Products1
{
	public partial class EditDeleteTaxPage : ContentPage
	{
		private  DeviceUser deviceUser;
		private  Tax tax ;
		public EditDeleteTaxPage (DeviceUser deviceUser, Tax tax)
		{
			InitializeComponent ();
			this.deviceUser = deviceUser;
			this.tax = tax;

			taxIDEntry.Text = tax.TaxID.ToString ();
			descriptionEntry.Text = tax.Description;
			taxPercentEntry.Text = tax.TaxPercent.ToString();

			backButton.Clicked += backButton_Clicked;
			editButton.Clicked += editButton_Clicked;
			deleteButton.Clicked += deleteButton_Clicked;
		}
		private async void backButton_Clicked(object sender, EventArgs e)
		{
			await Navigation.PushAsync (new TaxesPage (deviceUser));	
		}
		private async void editButton_Clicked(object sender, EventArgs e){

			if (string.IsNullOrEmpty (descriptionEntry.Text)) {
				await DisplayAlert ("Error", "Debe ingresar una descripción", "Aceptar");
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
				TaxID = this.tax.TaxID,
				Description = descriptionEntry.Text,
				TaxPercent = float.Parse(taxPercentEntry.Text)
			};
			var jsonRequest = JsonConvert.SerializeObject (tax);
			var content = new StringContent (jsonRequest, Encoding.UTF8, "text/json");


			string result;
			try {
				HttpClient client = new HttpClient ();
				client.BaseAddress = new Uri ("http://zulu-software.com");
				string url = string.Format ("/Z-Market/api/TaxesAPI/{0}", this.tax.TaxID);
				var response = await client.PutAsync (url, content);
				result = response.Content.ReadAsStringAsync ().Result;
			} catch (Exception) {
				DisplayAlert ("Error", "No hay conexión, intente mas tarde", "Aceptar");			
				waitActivityIndicator.IsRunning = false;
				return;	
			}  		

			waitActivityIndicator.IsRunning = false;	

			DisplayAlert ("Confirmación", "Impuesto Actualizado", "Aceptar");	
			await Navigation.PushAsync (new TaxesPage (deviceUser));
		}

		private async void deleteButton_Clicked(object sender, EventArgs e){
			var answer=await DisplayAlert ("Confirmación", "¿Está seguro de borrar el Impuesto?", "Sí","No");	
			if (!answer) {
				return;
			}
			waitActivityIndicator.IsRunning = true;
			string result;
			try {
				HttpClient client = new HttpClient ();
				client.BaseAddress = new Uri ("http://zulu-software.com");
				string url = string.Format ("/Z-Market/api/TaxesAPI/{0}",tax.TaxID);
				var response = await client.DeleteAsync(url);
				result = response.Content.ReadAsStringAsync ().Result;
			} catch (Exception) {
				DisplayAlert ("Error", "No hay conexión, intente mas tarde", "Aceptar");			
				waitActivityIndicator.IsRunning = false;
				return;	
			}  		


			waitActivityIndicator.IsRunning = false;
			DisplayAlert ("Confirmación", "Impuesto Eliminado", "Aceptar");	
			await Navigation.PushAsync (new TaxesPage (deviceUser));

		}
	}
}

