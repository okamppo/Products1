using System;
using System.Collections.Generic;

using Xamarin.Forms;
using Newtonsoft.Json;
using System.Net.Http;

namespace Products1
{
	public partial class SearchCategoryPage : ContentPage
	{
		private DeviceUser deviceUser;
		public SearchCategoryPage (DeviceUser deviceUser)
		{
			InitializeComponent ();
			this.deviceUser = deviceUser;
			searchButton.Clicked += searchButton_Clicked;
		}
		private async void searchButton_Clicked(object sender,EventArgs e){
			if(string.IsNullOrEmpty(categoryIDEntry.Text)){
				DisplayAlert ("Error","Debe ingresar un ID de categoría","Aceptar");
				categoryIDEntry.Focus ();
				return;
			}

			waitActivityIndicator.IsRunning = true;
			searchButton.IsEnabled = false;
			string result;
			try {
				HttpClient client = new HttpClient ();
				client.BaseAddress = new Uri ("http://zulu-software.com");
				string url = string.Format ("/Z-Market/api/CategoriesAPI/{0}",categoryIDEntry.Text);
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
				DisplayAlert ("Error", string.Format("La Categoría: {0} no existe",categoryIDEntry.Text),"Aceptar");			
				categoryIDEntry.Text = string.Empty;
				categoryIDEntry.Focus ();
				return;
			}

			var category = JsonConvert.DeserializeObject<Category> (result);
			await Navigation.PushAsync (new EditDeleteCategoryPage (this.deviceUser, category));
		}
	}
}

