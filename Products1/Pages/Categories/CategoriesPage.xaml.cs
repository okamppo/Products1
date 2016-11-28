using System;
using System.Collections.Generic;

using Xamarin.Forms;
using System.Net.Http;
using Newtonsoft.Json;

namespace Products1
{
	public partial class CategoriesPage : ContentPage
	{
		private List<Category> categories;
		private DeviceUser deviceUser;
		public CategoriesPage (DeviceUser deviceUser)
		{
			InitializeComponent ();
			this.deviceUser = deviceUser;
			categoriesListView.ItemTemplate = new DataTemplate (typeof(CategoryCell));
			this.LoadCategories ();

			newCategoryButton.Clicked += newCategoryButton_Clicked;
			findCategoryButton.Clicked += findCategoryButton_Clicked;
			categoriesListView.ItemTapped += categoriesListView_ItemTapped;
		}

		private async void newCategoryButton_Clicked(object sender, EventArgs e){
			await Navigation.PushAsync (new NewCategoryPage (this.deviceUser)); 
		}
		private async void findCategoryButton_Clicked(object sender, EventArgs e){
			await Navigation.PushAsync (new SearchCategoryPage(this.deviceUser)); //CAMBIAR PÁGINA DE NAVEGACIÓN
		}
		private async void categoriesListView_ItemTapped(object sender, ItemTappedEventArgs e){
			var category = e.Item as Category;
			await Navigation.PushAsync (new EditDeleteCategoryPage(this.deviceUser,category));	
		}


		private async void LoadCategories()
		{
			waitActivityIndicator.IsRunning = true;
			string result;
			try {
				HttpClient client = new HttpClient ();
				client.BaseAddress = new Uri ("http://zulu-software.com");
				string url = string.Format ("/Z-Market/api/CategoriesAPI");
				var response = await client.GetAsync (url);
				result = response.Content.ReadAsStringAsync ().Result;
			} catch (Exception) {
				DisplayAlert ("Error", "No hay conexión, intente mas tarde", "Aceptar");			
				waitActivityIndicator.IsRunning = false;
				return;	
			}  		

			waitActivityIndicator.IsRunning = false;	
			categories= JsonConvert.DeserializeObject<List<Category>>(result);
			categoriesListView.ItemsSource = categories;
		}
	}
}

