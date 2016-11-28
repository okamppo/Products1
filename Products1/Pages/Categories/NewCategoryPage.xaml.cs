using System;
using System.Collections.Generic;

using Xamarin.Forms;
using Newtonsoft.Json;
using System.Net.Http;
using System.Text;

namespace Products1
{
	public partial class NewCategoryPage : ContentPage
	{
		private DeviceUser deviceUser;

		public NewCategoryPage (DeviceUser deviceUser)
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
			waitActivityIndicator.IsRunning = true;

			var category = new Category {
				Description=descriptionEntry.Text
			};

			var jsonRequest = JsonConvert.SerializeObject (category);
			var content = new StringContent (jsonRequest, Encoding.UTF8,"text/json");


			string result;
			try {
				HttpClient client = new HttpClient ();
				client.BaseAddress = new Uri ("http://zulu-software.com");
				string url = string.Format ("/Z-Market/api/CategoriesAPI");
				var response = await client.PostAsync(url,content);
				result = response.Content.ReadAsStringAsync ().Result;
			} catch (Exception) {
				DisplayAlert ("Error", "No hay conexión, intente mas tarde", "Aceptar");			
				waitActivityIndicator.IsRunning = false;
				return;	
			}  		

			waitActivityIndicator.IsRunning = false;	
			await DisplayAlert ("Confirmación", "Categoría Creada", "Aceptar");
			await Navigation.PushAsync (new CategoriesPage (deviceUser)); 
		}
	}
}

