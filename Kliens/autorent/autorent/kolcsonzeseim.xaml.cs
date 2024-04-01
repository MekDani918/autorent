using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Text;
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
    /// Interaction logic for kolcsonzeseim.xaml
    /// </summary>
    public partial class kolcsonzeseim : Window
    {
        public kolcsonzeseim()
        {
            InitializeComponent();
        }

        private void MenuItem_Click_3(object sender, RoutedEventArgs e)
        {
            kolcsonzeseim kolcsonzeseim = new kolcsonzeseim();
            kolcsonzeseim.Show();        
            this.Close();
        }

        private void MenuItem_Click_2(object sender, RoutedEventArgs e)
        {
            autoklista autoklista = new autoklista();
            autoklista.Show();
            this.Close();
        }

        private void MenuItem_Click_1(object sender, RoutedEventArgs e)
        {
            MainWindow bejelenkezes = new MainWindow();
            bejelenkezes.Show();
            MainWindow.felhasznalotoken = "";
            this.Close();
        }

        private void datagrid_kolcsonzeseimlista_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {
                DataTable kolcsonzeseim = new DataTable();
                var client = new HttpClient();
                client.BaseAddress = new Uri(MainWindow.apiurl);
                var kolcsonzesek = client.GetAsync("/rentals").Result;
                if (kolcsonzesek.IsSuccessStatusCode)
                {
                    var valaszadat = kolcsonzesek.Content.ReadAsStringAsync().Result;
                    Debug.WriteLine(valaszadat);
                    kolcsonzeseim = datatablehelper.UseSystemTextJson(valaszadat);
                    datagrid_kolcsonzeseimlista.ItemsSource = kolcsonzeseim.DefaultView;
                }
            }
            catch
            {
                MessageBox.Show("Belső hiba", "Hiba", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}
