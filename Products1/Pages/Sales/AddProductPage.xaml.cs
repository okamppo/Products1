using System;
using System.Collections.Generic;

using Xamarin.Forms;
using System.Net.Http;
using Newtonsoft.Json;

namespace Products1
{
	public partial class AddProductPage : ContentPage
	{
		private List<Product> products;
		private DeviceUser deviceUser;
		private List<SaleDetail> details;

		public AddProductPage (DeviceUser deviceUser, List<SaleDetail> details)
		{
			InitializeComponent ();
			this.deviceUser = deviceUser;
			this.details = details;
			this.LoadProducts ();
			this.addButton.Clicked += addButton_Clicked;
		}

		private async void addButton_Clicked(object sender, EventArgs e)
		{
			if(string.IsNullOrEmpty(quantityEntry.Text))
			{
				await DisplayAlert("Error","Debe ingresar una cantidad","Aceptar");
				quantityEntry.Focus ();
				return;
			}

			float quant = float.Parse(quantityEntry.Text);
			if(quant<=0){
				await DisplayAlert("Error","Debe ingresar una cantidad mayor a cero{0}","Aceptar");
				quantityEntry.Focus ();
				return;
			}

			if(productPicker.SelectedIndex == -1)
			{
				await DisplayAlert("Error","Debe seleccionar un producto","Aceptar");
				productPicker.Focus ();
				return;
			}

			waitActivityIndicator.IsRunning = true;

			var detail = new SaleDetail {
				ProductID = this.products [productPicker.SelectedIndex].ProductID,
				Description = this.products [productPicker.SelectedIndex].Description,
				Price = this.products [productPicker.SelectedIndex].Price,
				Quantity = quant
			};					
							
			waitActivityIndicator.IsRunning = false;
			await Navigation.PushAsync (new NewSalePage (deviceUser, detail, details));
		}			

		private async void LoadProducts()
		{
			waitActivityIndicator.IsRunning = true;
			string result;
			try 
			{
				HttpClient client = new HttpClient ();
				client.BaseAddress = new Uri ("http://zulu-software.com");
				string url = string.Format ("/Z-Market/api/ProductsAPI");
				var response = await client.GetAsync (url);
				result = response.Content.ReadAsStringAsync ().Result;
			} 
			catch (Exception) 
			{
				DisplayAlert ("Error", "No hay conexión, intente mas tarde", "Aceptar").Wait();			
				waitActivityIndicator.IsRunning = false;
				return;	
			}

			this.products = JsonConvert.DeserializeObject<List<Product>>(result);
			foreach (var product in products) 
			{
				productPicker.Items.Add (product.Description);
			}
			waitActivityIndicator.IsRunning = false;		
		}
	}
}

