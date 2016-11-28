using System;
using System.Collections.Generic;

using Xamarin.Forms;

namespace Products1
{
	public partial class MenuPrincipalPage : ContentPage
	{
		private DeviceUser deviceUser;

		public MenuPrincipalPage (DeviceUser deviceUser)
		{
			InitializeComponent ();
			this.deviceUser = deviceUser;
			titleLabel.Text = string.Format ("Bienvenid@ {0}", this.deviceUser.NickName);

			productsButton.Clicked += productsButton_Clicked;
			customersButton.Clicked += customersButton_Clicked;
			suppliersButton.Clicked += suppliersButton_Clicked;
			categoriesButton.Clicked += categoriesButton_Clicked;
			taxesButton.Clicked += taxesButton_Clicked;
			ordersButton.Clicked += ordersButton_Clicked;
			shoppingsButton.Clicked += shoppingsButton_Clicked;
			salesButton.Clicked += salesButton_Clicked;
		}

		private async void productsButton_Clicked(object sender, EventArgs e)
		{
			await Navigation.PushAsync (new ProductsPage(this.deviceUser));
		}

		private async void categoriesButton_Clicked(object sender, EventArgs e)
		{
			await Navigation.PushAsync (new CategoriesPage(this.deviceUser));
		}

		private async void taxesButton_Clicked(object sender, EventArgs e)
		{
			await Navigation.PushAsync (new TaxesPage(this.deviceUser));
		}

		private async void customersButton_Clicked(object sender, EventArgs e)
		{
			await DisplayAlert ("Información", "Funcionalidad no disponible", "Aceptar");
		}

		private async void suppliersButton_Clicked(object sender, EventArgs e)
		{
			await DisplayAlert ("Información", "Funcionalidad no disponible", "Aceptar");
		}

		private async void ordersButton_Clicked(object sender, EventArgs e)
		{
			await DisplayAlert ("Información", "Funcionalidad no disponible", "Aceptar");
		}

		private async void shoppingsButton_Clicked(object sender, EventArgs e)
		{
			await DisplayAlert ("Información", "Funcionalidad no disponible", "Aceptar");
		}

		private async void salesButton_Clicked(object sender, EventArgs e)
		{
			await Navigation.PushAsync (new SalesPage(this.deviceUser));
		}
	}
}

