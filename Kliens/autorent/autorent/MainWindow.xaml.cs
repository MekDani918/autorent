using System;
using System.Collections.Generic;
using System.Linq;
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



namespace autorent
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void gomb_bejelentkezes_Click(object sender, RoutedEventArgs e)
        {
            if (textbox_felhasz.Text != "" && textbox_jelszo.Text != "")
            {
                //Validáció
                autoklista listaablak = new autoklista();
                listaablak.Show();
                this.Close();
            }
            else
            {
                MessageBox.Show("Kérjük Töltse ki a mezőket!", "Bejelntkezési hiba", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void textbox_jelszo_TextChanged(object sender, TextChangedEventArgs e)
        {

        }
    }
}
