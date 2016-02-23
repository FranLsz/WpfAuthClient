using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Microsoft.WindowsAzure.ActiveDirectory.Authentication;

namespace WpfAuthClient
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private AuthenticationResult auth;

        public MainWindow()
        {
            InitializeComponent();
        }

        private AuthenticationResult Autenticar()
        {
            var ctx = new AuthenticationContext("https://login.windows.net/demoauth015.onmicrosoft.com");
            var res = ctx.AcquireToken("https://demoauth015.onmicrosoft.com/ApiAuthActiveDirectory", "65b40b73-0384-4d7b-be47-9d63e7979615", "http://undominio/ok");

            return res;
        }

        private async Task<string> Operar(AuthenticationResult auth, string op, Operacion operacion)
        {
            var res = string.Empty;

            HttpClient cl = new HttpClient();
            cl.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", auth.AccessToken);

            var response =
                await
                    cl.PostAsync("https://apiauthactivedirectory15.azurewebsites.net/api/" + op,
                        new StringContent(operacion.ToJson(), Encoding.UTF8, "application/json"));

            if (response.IsSuccessStatusCode)
                res = await response.Content.ReadAsStringAsync();

            return res;
        }

        private async void button_Click(object sender, RoutedEventArgs e)
        {
            var op = new Operacion()
            {
                Op1 = Convert.ToInt32(textBox.Text),
                Op2 = Convert.ToInt32(textBox1.Text)
            };
            if (auth == null)
                auth = Autenticar();

            label.Content = await Operar(auth, "suma", op);
        }
    }
}
