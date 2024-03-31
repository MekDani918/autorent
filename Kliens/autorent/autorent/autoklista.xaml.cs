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
using System.Data.Common;


namespace autorent
{
    
    /// <summary>
    /// Interaction logic for autoklista.xaml
    /// </summary>
    public partial class autoklista : Window
    {
        public static DataTable alap;
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
                alap=UseSystemTextJson(valaszadat);
                datagrid_autoklista.ItemsSource = alap.DefaultView;
            }
        }
        //Függvény amely visszatér a DataTablelel
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

        private void listbox_kategoria_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            DataTable mostani = alap;       
            DataTable szurtrekodok=new DataTable();
            szurtrekodok.Columns.Add("id", typeof(System.Int32));
            szurtrekodok.Columns.Add("brand", typeof(System.String));
            szurtrekodok.Columns.Add("model", typeof(System.String));
            szurtrekodok.Columns.Add("category", typeof(System.String));
            szurtrekodok.Columns.Add("dailyPrice", typeof(System.Int32));        
            if (listbox_kategoria.SelectedIndex == 0)
            {
                datagrid_autoklista.ItemsSource = alap.DefaultView;
                return;
            }
            //Debug.WriteLine(listbox_kategoria.SelectedIndex);
            foreach (DataRow item in mostani.Rows) 
            {

                Debug.WriteLine(item["category"].ToString());
                //Debug.WriteLine(listbox_kategoria.SelectedValue);
                if (item["category"].ToString() == listbox_kategoria.SelectedValue.ToString())
                {
                    DataRow row = szurtrekodok.NewRow();
                    row["id"] = item["id"];
                    row["brand"] = item["brand"];
                    row["model"] = item["model"];
                    row["category"] = item["category"];
                    row["dailyPrice"]=item["dailyPrice"];
                    szurtrekodok.Rows.Add(row);

                    // szurtrekodok.ImportRow(item);
                }
            }                                         
            datagrid_autoklista.ItemsSource=szurtrekodok.DefaultView;
        }
    }
    
}
