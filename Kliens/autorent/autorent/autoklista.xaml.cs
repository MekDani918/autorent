using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
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
            var kategoriavalasz = client.GetAsync("/categories").Result;

            if (kategoriavalasz.IsSuccessStatusCode)
            {
                var valaszadat=kategoriavalasz.Content.ReadAsStringAsync().Result;
                var responseadat = JsonSerializer.Deserialize<List<responsekategoriak>>(valaszadat);
                foreach (var item in responseadat)
                {
                    listbox_kategoria.Items.Add(item.name);
                }
                
            }
            var autolistavalasz = client.GetAsync("/cars").Result;
            if(autolistavalasz.IsSuccessStatusCode) 
            {
                var valaszadat = autolistavalasz.Content.ReadAsStringAsync().Result;
                var responseadat=JsonSerializer.Deserialize < List<responseautok >> (valaszadat);
                DataTable dt=UseSystemTextJson(valaszadat);
                datagrid_autoklista.ItemsSource = dt.DefaultView;
            }
        }
        static DataTable? UseSystemTextJson(string sampleJson)
        {
            DataTable? dataTable = new();
            if (string.IsNullOrWhiteSpace(sampleJson))
            {
                return dataTable;
            }

            JsonElement data = JsonSerializer.Deserialize<JsonElement>(sampleJson);
            if (data.ValueKind != JsonValueKind.Array)
            {
                return dataTable;
            }

            var dataArray = data.EnumerateArray();
            JsonElement firstObject = dataArray.First();

            var firstObjectProperties = firstObject.EnumerateObject();
            foreach (var element in firstObjectProperties)
            {
                dataTable.Columns.Add(element.Name);
            }

            foreach (var obj in dataArray)
            {
                var objProperties = obj.EnumerateObject();
                DataRow newRow = dataTable.NewRow();
                foreach (var item in objProperties)
                {
                    newRow[item.Name] = item.Value;
                }
                dataTable.Rows.Add(newRow);
            }

            return dataTable;
        }

        private void MenuItem_Click_2(object sender, RoutedEventArgs e)
        {
            autoklista autoklistja = new autoklista();
            autoklistja.Show();          
            this.Close();
        }
    }
    
}
