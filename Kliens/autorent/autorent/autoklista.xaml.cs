using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace autorent
{
    /// <summary>
    /// Interaction logic for autoklista.xaml
    /// </summary>
    public partial class autoklista : Window
    {
        public autoklista()
        {
            InitializeComponent();
        }

        private void MenuItem_Click(object sender, RoutedEventArgs e)
        {

        }

        private void MenuItem_Click_1(object sender, RoutedEventArgs e)
        {
            MainWindow bejelenkezes=new MainWindow();
            bejelenkezes.Show();
            MainWindow.felhasznalotoken = "";
            this.Close();
            
        }

        private void window_autoklistadefault_load_Loaded(object sender, RoutedEventArgs e)
        {
            var client = new HttpClient();
            client.BaseAddress = new Uri(MainWindow.apiurl);
            var valasz = client.GetAsync("/categories").Result;

            if (valasz.IsSuccessStatusCode)
            {
                var valaszadat=valasz.Content.ReadAsStringAsync().Result;
                var responseadat = JsonSerializer.Deserialize<List<responsekategoriak>>(valaszadat);
                foreach (var item in responseadat)
                {
                    listbox_kategoria.Items.Add(item.name);
                }
                
            }
        }
    }
}
