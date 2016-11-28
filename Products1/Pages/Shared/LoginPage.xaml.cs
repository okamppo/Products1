using System;
using System.Collections.Generic;

using Xamarin.Forms;
using System.Net.Http;
using Newtonsoft.Json;
using Products1.Services;
using System.Threading.Tasks;

namespace Products1
{
	public partial class LoginPage : ContentPage
	{
        private AzureClient _client;
		public LoginPage ()
		{
			InitializeComponent ();
			enterButton.Clicked += enterButton_Clicked;
            newButton.Clicked += newButton_Clicked;
            _client = new AzureClient();
		}

        public async Task cleanLocalData()
        {
            await _client.CleanData();
        }

        private async void newButton_Clicked(object sender, EventArgs e)
        {
            waitActivityIndicator.IsRunning = true;
            string[] nicknames = { "luis", "miguel", "francisco", "antonio",
                                "sofia", "camila", "valentina", "isabella", "ximena"};
            string[] passwords = { "a123", "b123", "c123", "d123", "e123" };
            
            Random rdn = new Random(DateTime.Now.Millisecond);
            try
            {
                DeviceUser user = new DeviceUser();
                user.NickName = nicknames[rdn.Next(0, 8)];
                user.Password = passwords[rdn.Next(0, 4)];
                user.DeviceUserID = 1;
                await _client.AddDeviceUser(user);
                await DisplayAlert("Exito", "usuario creado", "Aceptar");
            }
            catch (Exception ex)
            {            
                await DisplayAlert("Error", ex.Message, "Aceptar");
                waitActivityIndicator.IsRunning = false;
                return;
            }
            waitActivityIndicator.IsRunning = false;
        }

        private async void enterButton_Clicked(Object sender, EventArgs e)
		{
			if (string.IsNullOrEmpty (userEntry.Text)) 
			{
				await DisplayAlert ("Error", "Debe ingresar un usuario", "Aceptar");
				userEntry.Focus ();
				return;
			}
			if (string.IsNullOrEmpty (passwordEntry.Text))
			{
				await DisplayAlert ("Error", "Debe ingresar una Clave", "Aceptar");
				passwordEntry.Focus ();
				return;
			}
			waitActivityIndicator.IsRunning = true;
            string result;

            var list = new List<DeviceUser>();
			try 
			{
				enterButton.IsEnabled = false;
                var devRes = await _client.GetDeviceUsers();
                foreach (var item in devRes)
                {
                    list.Add(item);
                }
                //HttpClient client = new HttpClient ();
                //client.BaseAddress = new Uri ("http://zulu-software.com");
                //string url = string.Format ("/Z-Market/api/DeviceUsersAPI/{0}/{1}",userEntry.Text,passwordEntry.Text);
                //var response = await client.GetAsync (url);
                //result = response.Content.ReadAsStringAsync ().Result;
                enterButton.IsEnabled = true;
			} 
			catch (Exception ex) 
			{
                result = ex.Message;
				DisplayAlert ("Error", "No hay conexión, intente mas tarde", "Aceptar").Wait();
				enterButton.IsEnabled = true;
				waitActivityIndicator.IsRunning = false;
				return;	
			}  		

			waitActivityIndicator.IsRunning = false;

            if (list == null || list.Count == 0)
            {
                await DisplayAlert("Error", "Usuario o clave no válidos", "Aceptar");
                passwordEntry.Text = string.Empty;
                passwordEntry.Focus();
                return;
            }

            //if (string.IsNullOrEmpty (result) || result == "null") 
            //{
            //	await DisplayAlert ("Error", "Usuario o clave no válidos", "Aceptar");
            //	passwordEntry.Text = string.Empty;
            //	passwordEntry.Focus ();
            //	return;
            //}

            //var deviceUser = JsonConvert.DeserializeObject<DeviceUser> (result);
            //await Navigation.PushAsync (new MenuPrincipalPage (deviceUser));
            var deviceUsers = list.ToArray();
            await Navigation.PushAsync(new MenuPrincipalPage(deviceUsers[0]));
        }
	}
}
