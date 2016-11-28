using System;
using System.Collections.Generic;

using Xamarin.Forms;
using System.Net.Http;
using Newtonsoft.Json;
using System.Text;
using System.Linq;

namespace Products1
{
	public partial class NewSalePage : ContentPage
	{
		private List<Customer> customers;
		private List<SaleDetail> details;
		private DeviceUser deviceUser;

		public NewSalePage (DeviceUser deviceUser, SaleDetail product = null, List<SaleDetail> saleDetails = null)
		{
			InitializeComponent ();
			this.deviceUser = deviceUser;
			this.details = saleDetails;
			this.LoadCustomers ();
			if (product != null) 
			{
				if (this.details != null) {
					var det = details.Find (p => p.ProductID == product.ProductID);
					if (det == null) {
						this.details.Add (product);
					} 
					else
					{
						det.Quantity += product.Quantity;
					}
				} 
				else 
				{
					details = new List<SaleDetail> ();
					details.Add (product);
				}
			}
			productsListView.ItemTemplate = new DataTemplate (typeof(NewSaleCell));
			productsListView.ItemsSource= this.details;
			this.newButton.Clicked += newButton_Clicked;
			this.saveButton.Clicked  += saveButton_Clicked;
		}

		private async void saveButton_Clicked (object sender, EventArgs e)
		{
			if(customerPicker.SelectedIndex == -1)
			{
				await DisplayAlert("Error","Debe seleccionar una categoria","Aceptar");
				customerPicker.Focus ();
				return;
			}

			waitActivityIndicator.IsRunning = true;

			var sale = new Sale {
				CustomerID=this.customers[customerPicker.SelectedIndex].CustomerID,
				DateSale = DateTime.Now
			};

			var jsonRequest = JsonConvert.SerializeObject (sale);
			var content1 = new StringContent (jsonRequest, Encoding.UTF8,"text/json");


			string result;
			try 
			{
				HttpClient client = new HttpClient ();
				client.BaseAddress = new Uri ("http://zulu-software.com");
				string url = string.Format ("/Z-Market/api/SalesAPI");
				var response = await client.PostAsync(url,content1);
				result = response.Content.ReadAsStringAsync ().Result;

				HttpClient client2 = new HttpClient ();
				client2.BaseAddress = new Uri("http://zulu-software.com");
				string url2 = string.Format("/Z-Market/api/SalesAPI/");
				var response2 = await client2.GetAsync(url2);
				string result2 = response2.Content.ReadAsStringAsync().Result;

				List<Sale> sales = new List<Sale>();
				sales = JsonConvert.DeserializeObject<List<Sale>>(result2);

				var sa = sales.Where(s => s.CustomerID == this.customers[customerPicker.SelectedIndex].CustomerID).LastOrDefault();

				foreach(var item in details)
				{
					var detail = new SaleDetail{
						SaleID  = sa.SaleID,
						ProductID = item.ProductID,
						Description = item.Description,
						Price = item.Price,
						Quantity = item.Quantity
					};

					var jsonRequest2 = JsonConvert.SerializeObject (detail);
					var content2 = new StringContent (jsonRequest2, Encoding.UTF8,"text/json");

					HttpClient client3 = new HttpClient ();
					client3.BaseAddress = new Uri ("http://zulu-software.com");
					string url3 = string.Format ("/Z-Market/api/SaleDetailsAPI");
					var response3 = await client.PostAsync(url3,content2);
					string result3 = response3.Content.ReadAsStringAsync ().Result;

					HttpClient client4 = new HttpClient ();
					client4.BaseAddress = new Uri("http://zulu-software.com");
					string url4 = string.Format("/Z-Market/api/ProductsAPI/{0}",item.ProductID);
					var response4 = await client4.GetAsync(url4);
					string result4 = response4.Content.ReadAsStringAsync().Result;

					Product prod = new Product();
					prod = JsonConvert.DeserializeObject<Product>(result4);

					prod.Stock -= item.Quantity;
					var jsonRequest4 = JsonConvert.SerializeObject (prod);
					var content4 = new StringContent (jsonRequest4, Encoding.UTF8,"text/json");

					string result5;
					try 
					{
						HttpClient client5 = new HttpClient ();
						client5.BaseAddress = new Uri ("http://zulu-software.com");
						string url5 = string.Format ("/Z-Market/api/ProductsAPI/{0}",item.ProductID);
						var response5 = await client.PutAsync(url5,content4);
						result5 = response5.Content.ReadAsStringAsync ().Result;
					} 
					catch (Exception) 
					{
						DisplayAlert ("Error", "No hay conexión, intente mas tarde", "Aceptar").Wait();			
						waitActivityIndicator.IsRunning = false;
						return;	
					}
				}
			} 
			catch (Exception) 
			{
				DisplayAlert ("Error", "No hay conexión, intente mas tarde", "Aceptar").Wait();			
				waitActivityIndicator.IsRunning = false;
				return;	
			}

			waitActivityIndicator.IsRunning = false;	
			await DisplayAlert ("Confirmación", "Venta Creada", "Aceptar");
			await Navigation.PushAsync (new SalesPage (deviceUser)); 
		}

		private async void newButton_Clicked(object sender, EventArgs e)
		{
			await Navigation.PushAsync (new AddProductPage (deviceUser, this.details));
		}			

		private async void LoadCustomers()
		{
			waitActivityIndicator.IsRunning = true;
			string result;
			try 
			{
				HttpClient client = new HttpClient ();
				client.BaseAddress = new Uri ("http://zulu-software.com");
				string url = string.Format ("/Z-Market/api/CustomersAPI");
				var response = await client.GetAsync (url);
				result = response.Content.ReadAsStringAsync ().Result;
			} 
			catch (Exception) 
			{
				DisplayAlert ("Error", "No hay conexión, intente mas tarde", "Aceptar").Wait();			
				waitActivityIndicator.IsRunning = false;
				return;	
			}

			this.customers = JsonConvert.DeserializeObject<List<Customer>>(result);
			foreach (var customer in customers) 
			{
				customerPicker.Items.Add (customer.FullName);
			}

			waitActivityIndicator.IsRunning = false;
		}
	}
}

