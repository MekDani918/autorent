using System.Globalization;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Text.Json;
using System.Net.Http;

namespace autorent
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    
    public partial class MainWindow : Window
    {
        public static string felhasznalotoken="";
        public const string apiurl = "http://127.0.0.1:3000";
        public MainWindow()
        {
            InitializeComponent();
        }

        private void gomb_bejelentkezes_Click(object sender, RoutedEventArgs e)
        {
            if (textbox_felhasz.Text != "" && textbox_password.Text != "")
            {
                var postuser = new userpost { username = textbox_felhasz.Text, password = textbox_password.Text };
                var client = new HttpClient();
                client.BaseAddress = new Uri(apiurl);
                //json konvertálás
                var json=JsonSerializer.Serialize(postuser);
                var postdata=new StringContent(json, Encoding.UTF8, "application/json");

                var valasz = client.PostAsync("/login", postdata).Result;

                if (valasz.IsSuccessStatusCode) 
                { 
                    var valaszadat=valasz.Content.ReadAsStringAsync().Result;
                    var postvalasz=JsonSerializer.Deserialize<userresponse>(valaszadat);
                    felhasznalotoken = postvalasz.token;
                    string message=postvalasz.message;
                    MessageBox.Show("Üdvözöljük "+ felhasznalotoken + "!", "Bejelentkezés");
                    //Validáció
                    autoklista listaablak = new autoklista();
                    listaablak.Show();
                    this.Close();
                }
                else
                {
                    MessageBox.Show("Hibás felhasználónév vagy jelszó! Hibakód:"+valasz.StatusCode, "Bejelntkezési hiba", MessageBoxButton.OK, MessageBoxImage.Error);
                }
                
            }
            else
            {
                MessageBox.Show("Kérjük Töltse ki a mezőket!", "Bejelntkezési hiba", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
    //Szöveg placeholder! így cool
    //public class Converter : IValueConverter
    //{
    //    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    //    {
    //        double num;
    //        if (Double.TryParse(value.ToString(), out num))
    //            return num;
    //        return "";
    //    }

    //    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    //    {
    //        throw new NotImplementedException();
    //    }
    //}
}