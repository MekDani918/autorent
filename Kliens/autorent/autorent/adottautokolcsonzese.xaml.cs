using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Text.Json;
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
    /// Interaction logic for adottautokolcsonzese.xaml
    /// </summary>
    public partial class adottautokolcsonzese : Window
    {

        static string adottautoid;
        static int adottautonapiara=0;
        static string arcontent = "";
        static string postvalaszmessage = "";
        
        public adottautokolcsonzese(string _adottautoadat)
        {
            InitializeComponent();
            adottautoid = _adottautoadat;
            arcontent = label_foglalasara.Content.ToString();
        }

        private void MenuItem_Click_1(object sender, RoutedEventArgs e)
        {
            MainWindow bejelenkezes = new MainWindow();
            bejelenkezes.Show();
            MainWindow.felhasznalotoken = "";
            this.Close();
        }

        private void MenuItem_Click_3(object sender, RoutedEventArgs e)
        {
            kolcsonzeseim kolcsonzeseim = new kolcsonzeseim();
            kolcsonzeseim.Show();
            this.Close();
        }

        private void MenuItem_Click_2(object sender, RoutedEventArgs e)
        {
            autoklista autoklistja = new autoklista();
            autoklistja.Show();
            this.Close();
        }

        private void grid_adottautokolcsonzes_Loaded(object sender, RoutedEventArgs e)
        {
            
            DataTable adottauto;
            var client = new HttpClient();
            client.BaseAddress = new Uri(MainWindow.apiurl);
            var autolistavalasz = client.GetAsync("/cars/"+adottautoid.ToString()).Result;
            Debug.WriteLine(autolistavalasz);
            if (autolistavalasz.IsSuccessStatusCode)
            {
                var valaszadat = autolistavalasz.Content.ReadAsStringAsync().Result;
                Debug.WriteLine(valaszadat);
                var postvalasz = JsonSerializer.Deserialize<responseautok>(valaszadat);
                adottautonapiara = postvalasz.dailyPrice;
                label_autonev.Content = postvalasz.brand +" " +postvalasz.model;
                label_autokategoria.Content = postvalasz.category;
                for (int i = 0;i<postvalasz.unavailableDates.Count;i++) 
                {
                    calandar_foglaltidopontok.BlackoutDates.Add(new CalendarDateRange(postvalasz.unavailableDates[i]));
                    datepicker_tol.BlackoutDates.Add(new CalendarDateRange(postvalasz.unavailableDates[i]));
                    datepicker_ig.BlackoutDates.Add(new CalendarDateRange(postvalasz.unavailableDates[i]));
                }
            }          
        }

        private void datepicker_tol_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {
            int ar = 0;
            if (datepicker_tol.SelectedDate > datepicker_ig.SelectedDate)
            {
                MessageBox.Show("Nem lehet a kezdődátum nagyobb mint a végdátum", "Dátumkiválasztási hiba");
                datepicker_ig.SelectedDate = null;
                gomb_foglalas.IsEnabled = false;
            }
            else if (datepicker_tol.SelectedDate == null || datepicker_ig.SelectedDate == null)
            {
                ar = 0;
                string szoveg = arcontent;
                szoveg += " " + ar;
                label_foglalasara.Content = szoveg;
                gomb_foglalas.IsEnabled = false;
            }
            else if (datepicker_tol.SelectedDate == datepicker_ig.SelectedDate)
            {

                ar = adottautonapiara;
                string szoveg = arcontent;
                szoveg += " " + ar;
                label_foglalasara.Content = szoveg;
                gomb_foglalas.IsEnabled = true;
            }
            else
            {
                int napokszama = (datepicker_ig.SelectedDate - datepicker_tol.SelectedDate).Value.Days+1;
                ar = napokszama * adottautonapiara;
                string szoveg = arcontent;
                szoveg += " " + ar;
                label_foglalasara.Content = szoveg;
                gomb_foglalas.IsEnabled = true;
            }
        }

        private void datepicker_ig_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {
            int ar = 0;
            if (datepicker_tol.SelectedDate > datepicker_ig.SelectedDate)
            {
                MessageBox.Show("Nem lehet a kezdődátum nagyobb mint a végdátum", "Dátumkiválasztási hiba");
                datepicker_ig.SelectedDate = null;
                gomb_foglalas.IsEnabled = false;
            }
            else if (datepicker_tol.SelectedDate == null || datepicker_ig.SelectedDate == null)
            {
                ar = 0;
                string szoveg = arcontent;
                szoveg += " " + ar;
                label_foglalasara.Content = szoveg;
                gomb_foglalas.IsEnabled = false;
            }
            else if (datepicker_tol.SelectedDate == datepicker_ig.SelectedDate)
            {

                ar = adottautonapiara;
                string szoveg = arcontent;
                szoveg += " " + ar;
                label_foglalasara.Content = szoveg;
                gomb_foglalas.IsEnabled = true;
            }
            else
            {
                int napokszama = (datepicker_ig.SelectedDate - datepicker_tol.SelectedDate).Value.Days+1;
                ar = napokszama * adottautonapiara;
                string szoveg = arcontent;
                szoveg += " " + ar;
                label_foglalasara.Content = szoveg;
                gomb_foglalas.IsEnabled = true;
            }
        }

        private void gomb_foglalas_Click(object sender, RoutedEventArgs e)
        {
            if (datepicker_tol.SelectedDate != null && datepicker_tol.SelectedDate != null)
            {
                var postuser = new postkolcsonzes { carId = Convert.ToInt32(adottautoid), from = (((DateTimeOffset)datepicker_tol.SelectedDate).ToUnixTimeSeconds()).ToString(),to= (((DateTimeOffset)datepicker_ig.SelectedDate).ToUnixTimeSeconds()).ToString() };
                var client = new HttpClient();
                client.BaseAddress = new Uri(MainWindow.apiurl);
                //json konvertálás
                var json = JsonSerializer.Serialize(postuser);
                var postdata = new StringContent(json, Encoding.UTF8, "application/json");

                var valasz = client.PostAsync("/rentals", postdata).Result;

                if (valasz.IsSuccessStatusCode)
                {
                    var valaszadat = valasz.Content.ReadAsStringAsync().Result;
                    var postvalasz = JsonSerializer.Deserialize<responsekolcsonzes>(valaszadat);
                    postvalaszmessage = postvalasz.message;
                    int id=postvalasz.id;
                    MessageBox.Show(postvalaszmessage + "!", "Kölcsönzés");
                    //Vissza
                    autoklista listaablak = new autoklista();
                    listaablak.Show();
                    this.Close();
                }
                else
                {
                    MessageBox.Show(postvalaszmessage + valasz.StatusCode, "Kölcsönzési Hiba", MessageBoxButton.OK, MessageBoxImage.Error);
                }

            }           
        }
    }
}
