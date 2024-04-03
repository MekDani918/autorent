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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace autorent.Views
{
    /// <summary>
    /// Interaction logic for RentalsView.xaml
    /// </summary>
    public partial class RentalsView : UserControl
    {
        public RentalsView()
        {
            InitializeComponent();
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
