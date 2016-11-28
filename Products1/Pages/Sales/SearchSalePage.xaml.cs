using System;
using System.Collections.Generic;
using System.Linq;

using Xamarin.Forms;

namespace Products1
{
	public partial class SearchSalePage : ContentPage
	{
		private DeviceUser deviceUser;
		private List<SaleView> salesView;

		public SearchSalePage (DeviceUser deviceUser, List<SaleView> salesView)
		{
			InitializeComponent ();
			this.deviceUser = deviceUser;
			this.salesView = salesView;
			this.titleLabel.Text = this.deviceUser.NickName;
			this.filterPicker.Items.Add ("Venta");
			this.filterPicker.Items.Add ("Cédula cliente");
			searchButton.Clicked += searchButton_Clicked;
		}

		private async void searchButton_Clicked(object sender,EventArgs e)
		{
			if(string.IsNullOrEmpty(IDEntry.Text))
			{
				await DisplayAlert ("Error","Debe ingresar un criterio de búsqueda","Aceptar");
				IDEntry.Focus ();
				return;
			}

			int ID = int.Parse(IDEntry.Text);
			if(ID < 0){
				await DisplayAlert("Error","Debe ingresar un criterio mayor a cero{0}","Aceptar");
				IDEntry.Focus ();
				return;
			}

			if(filterPicker.SelectedIndex == -1)
			{
				await DisplayAlert("Error","Debe seleccionar un filtro","Aceptar");
				filterPicker.Focus ();
				return;
			}
			waitActivityIndicator.IsRunning = true;
			searchButton.IsEnabled = false;
			try 
			{
				var sale = new SaleView();
				if(filterPicker.SelectedIndex == 0)
				{					
					sale = this.salesView.Where(s => s.Sale.SaleID == ID).FirstOrDefault();
				}
				else if (filterPicker.SelectedIndex == 1)
				{
					sale = this.salesView.Where(s => s.Customer.CustomerID == ID).FirstOrDefault();
				}
				else
				{
					await DisplayAlert ("Error", string.Format("La búsqueda: {0} no existe",IDEntry.Text),"Aceptar");			
					IDEntry.Text = string.Empty;
					IDEntry.Focus ();
					return;
				}
				searchButton.IsEnabled = true;
				waitActivityIndicator.IsRunning = false;
				await Navigation.PushAsync (new SaleDetailsPage(this.deviceUser, sale));
			} 
			catch (Exception) 
			{
				DisplayAlert ("Error", "No hay conexión, intente mas tarde", "Aceptar").Wait();			
				waitActivityIndicator.IsRunning = false;
				searchButton.IsEnabled = true;
				return;	
			}  						
		}
	}
}

