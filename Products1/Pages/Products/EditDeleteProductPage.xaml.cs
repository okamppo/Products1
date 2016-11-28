using System;
using System.Collections.Generic;

using Xamarin.Forms;
using System.Net.Http;
using Newtonsoft.Json;
using System.Text;

namespace Products1
{
	public partial class EditDeleteProductPage : ContentPage
	{
		private  DeviceUser deviceUser;
		private Product product;
		private List<Category> categories;
		private List<Tax> taxes;

	
		public EditDeleteProductPage (DeviceUser deviceUser,Product product)
		{
			InitializeComponent();
			this.deviceUser = deviceUser;
			this.product = product;
			this.LoadCategories();
			this.LoadTaxes();

			productIDEntry.Text = product.ProductID.ToString();
			descriptionEntry.Text = product.Description;
			priceEntry.Text = product.Price.ToString();
			lastBuyDatePicker.Date = product.LastBuy;
			stockEntry.Text = product.Stock.ToString();
			remarksEntry.Text = product.Remarks;

			editButton.Clicked += editButton_Clicked;
			deleteButton.Clicked += deleteButton_Clicked;
			backButton.Clicked += backButton_Clicked;
		}

		private async void backButton_Clicked(object sender, EventArgs e)
		{
			await Navigation.PushAsync (new ProductsPage (deviceUser));	
		}

		private async void editButton_Clicked(object sender, EventArgs e)
		{		
			if(string.IsNullOrEmpty(descriptionEntry.Text))
			{
				await DisplayAlert("Error","Debe ingresar una descripción","Aceptar");
				descriptionEntry.Focus ();
				return;
			}
			if(string.IsNullOrEmpty(priceEntry.Text))
			{
				await DisplayAlert("Error","Debe ingresar un precio","Aceptar");
				descriptionEntry.Focus ();
				return;
			}
			decimal price = decimal.Parse(priceEntry.Text);
			if(price <= 0)
			{
				await DisplayAlert("Error","Debe ingresar un precio mayor a cero{0}","Aceptar");
				priceEntry.Focus ();
				return;
			}
			float stock = 0;
			if(!string.IsNullOrEmpty(stockEntry.Text))
			{
				stock = float.Parse(stockEntry.Text);
				if(stock < 0)
				{
					await DisplayAlert("Error","Debe ingresar un inventario mayor o igual cero{0}","Aceptar");
					priceEntry.Focus();
					return;
				}
			}

			if(categoryPicker.SelectedIndex == -1)
			{
				await DisplayAlert("Error","Debe seleccionar una categoria","Aceptar");
				categoryPicker.Focus ();
				return;
			}
			if(taxPicker.SelectedIndex == -1)
			{
				await DisplayAlert("Error","Debe seleccionar un impuesto","Aceptar");
				taxPicker.Focus ();
				return;
			}
			waitActivityIndicator.IsRunning = true;

			var product = new Product {
				ProductID=this.product.ProductID,
				CategoryID=this.categories[categoryPicker.SelectedIndex].CategoryID,
				Description=descriptionEntry.Text,
				LastBuy=lastBuyDatePicker.Date,
				Price=price,
				Remarks=remarksEntry.Text,
				Stock=stock,
				TaxID=this.taxes[taxPicker.SelectedIndex].TaxID
			};

			var jsonRequest = JsonConvert.SerializeObject (product);
			var content = new StringContent (jsonRequest, Encoding.UTF8,"text/json");


			string result;
			try 
			{
				HttpClient client = new HttpClient ();
				client.BaseAddress = new Uri ("http://zulu-software.com");
				string url = string.Format ("/Z-Market/api/ProductsAPI/{0}",this.product.ProductID);
				var response = await client.PutAsync(url,content);
				result = response.Content.ReadAsStringAsync ().Result;
			} 
			catch (Exception) 
			{
				DisplayAlert ("Error", "No hay conexión, intente mas tarde", "Aceptar").Wait();			
				waitActivityIndicator.IsRunning = false;
				return;	
			}  		


			waitActivityIndicator.IsRunning = false;	

			DisplayAlert ("Confirmación", "Producto Actualizado", "Aceptar").Wait();	
			await Navigation.PushAsync (new ProductsPage (deviceUser));
		}

		private async void deleteButton_Clicked(object sender, EventArgs e)
		{
			var answer=await DisplayAlert ("Confirmación", "¿Está seguro de borrar el producto?", "Sí","No");	
			if (!answer) 
			{
				return;
			}

			waitActivityIndicator.IsRunning = true;
			string result;
			try 
			{
				HttpClient client = new HttpClient ();
				client.BaseAddress = new Uri ("http://zulu-software.com");
				string url = string.Format ("/Z-Market/api/ProductsAPI/{0}",product.ProductID);
				var response = await client.DeleteAsync(url);
				result = response.Content.ReadAsStringAsync ().Result;
			} 
			catch (Exception) 
			{
				DisplayAlert ("Error", "No hay conexión, intente mas tarde", "Aceptar").Wait();			
				waitActivityIndicator.IsRunning = false;
				return;	
			}  		

			waitActivityIndicator.IsRunning = false;
			DisplayAlert ("Confirmación", "Producto Borrado", "Aceptar").Wait();	
			await Navigation.PushAsync (new ProductsPage (deviceUser));

		}


		private async void LoadCategories()
		{
			waitActivityIndicator.IsRunning = true;
			string result;
			try 
			{
				HttpClient client = new HttpClient ();
				client.BaseAddress = new Uri ("http://zulu-software.com");
				string url = string.Format ("/Z-Market/api/CategoriesAPI");
				var response = await client.GetAsync (url);
				result = response.Content.ReadAsStringAsync ().Result;
			} 
			catch (Exception) {
				DisplayAlert ("Error", "No hay conexión, intente mas tarde", "Aceptar").Wait();			
				waitActivityIndicator.IsRunning = false;
				return;	
			}  		

			this.categories= JsonConvert.DeserializeObject<List<Category>>(result);
			foreach (var category in categories) 
			{
				categoryPicker.Items.Add (category.Description);
			}

			categoryPicker.SelectedIndex = this.GetIndexCategory (this.product.CategoryID);

			waitActivityIndicator.IsRunning = false;	
		}

		private int GetIndexCategory(int categoryID)
		{
			for (int i = 0; i < categories.Count; i++) 
			{
				if(this.product.CategoryID==categories[i].CategoryID)
				{
					return i;
				}
			}
			return -1;
		}

		private async void LoadTaxes()
		{
			waitActivityIndicator.IsRunning = true;
			string result;
			try 
			{
				HttpClient client = new HttpClient ();
				client.BaseAddress = new Uri ("http://zulu-software.com");
				string url = string.Format ("/Z-Market/api/TaxesAPI");
				var response = await client.GetAsync (url);
				result = response.Content.ReadAsStringAsync ().Result;
			} 
			catch (Exception) 
			{
				DisplayAlert ("Error", "No hay conexión, intente mas tarde", "Aceptar").Wait();			
				waitActivityIndicator.IsRunning = false;
				return;	
			}

			taxes= JsonConvert.DeserializeObject<List<Tax>>(result);
			foreach (var tax in taxes) 
			{
				taxPicker.Items.Add (tax.Description);
			}

			taxPicker.SelectedIndex = this.GetIndexTax (this.product.TaxID);
			waitActivityIndicator.IsRunning = false;
		}

		private int GetIndexTax(int taxID)
		{
			for (int i = 0; i < taxes.Count; i++) 
			{
				if(this.product.TaxID == taxes[i].TaxID)
				{
					return i;
				}
			}
			return -1;
		}

	}
}

