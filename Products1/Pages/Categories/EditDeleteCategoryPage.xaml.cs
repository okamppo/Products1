using System;
using System.Collections.Generic;

using Xamarin.Forms;
using System.Net.Http;
using System.Text;
using Newtonsoft.Json;

namespace Products1
{
	public partial class EditDeleteCategoryPage : ContentPage
	{
		private  DeviceUser deviceUser;
		private Category category;

		public EditDeleteCategoryPage (DeviceUser deviceUser,Category category)
		{
			InitializeComponent ();
			this.deviceUser = deviceUser;
			this.category = category;

			categoryIDEntry.Text = category.CategoryID.ToString ();
			descriptionEntry.Text = category.Description;

			backButton.Clicked += backButton_Clicked;
			editButton.Clicked += editButton_Clicked;
			deleteButton.Clicked += deleteButton_Clicked;

		}
		private async void backButton_Clicked(object sender, EventArgs e)
		{
			await Navigation.PushAsync (new CategoriesPage (deviceUser));	
		}
		private async void editButton_Clicked(object sender, EventArgs e){

			if (string.IsNullOrEmpty (descriptionEntry.Text)) {
				await DisplayAlert ("Error", "Debe ingresar una descripción", "Aceptar");
				descriptionEntry.Focus ();
				return;
			}
			waitActivityIndicator.IsRunning = true;

			var category = new Category {
				CategoryID = this.category.CategoryID,
				Description = descriptionEntry.Text
			};
			var jsonRequest = JsonConvert.SerializeObject (category);
			var content = new StringContent (jsonRequest, Encoding.UTF8, "text/json");


			string result;
			try {
				HttpClient client = new HttpClient ();
				client.BaseAddress = new Uri ("http://zulu-software.com");
				string url = string.Format ("/Z-Market/api/CategoriesAPI/{0}", this.category.CategoryID);
				var response = await client.PutAsync (url, content);
				result = response.Content.ReadAsStringAsync ().Result;
			} catch (Exception) {
				DisplayAlert ("Error", "No hay conexión, intente mas tarde", "Aceptar");			
				waitActivityIndicator.IsRunning = false;
				return;	
			}  		

			waitActivityIndicator.IsRunning = false;	

			DisplayAlert ("Confirmación", "Producto Actualizado", "Aceptar");	
			await Navigation.PushAsync (new CategoriesPage (deviceUser));
		}

		private async void deleteButton_Clicked(object sender, EventArgs e){
			var answer=await DisplayAlert ("Confirmación", "¿Está seguro de borrar la Categoría?", "Sí","No");	
			if (!answer) {
				return;
			}
			waitActivityIndicator.IsRunning = true;
			string result;
			try {
				HttpClient client = new HttpClient ();
				client.BaseAddress = new Uri ("http://zulu-software.com");
				string url = string.Format ("/Z-Market/api/CategoriesAPI/{0}",category.CategoryID);
				var response = await client.DeleteAsync(url);
				result = response.Content.ReadAsStringAsync ().Result;
			} catch (Exception) {
				DisplayAlert ("Error", "No hay conexión, intente mas tarde", "Aceptar");			
				waitActivityIndicator.IsRunning = false;
				return;	
			}  		


			waitActivityIndicator.IsRunning = false;
			DisplayAlert ("Confirmación", "Categoría Borrada", "Aceptar");	
			await Navigation.PushAsync (new CategoriesPage (deviceUser));

		}
	}
}

