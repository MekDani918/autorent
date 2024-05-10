using System.Windows;
using System.Windows.Controls;
using autorent.ViewModels;

namespace autorent.Views
{
    /// <summary>
    /// Interaction logic for LoginView.xaml
    /// </summary>
    public partial class RegisterView : UserControl
    {
        public RegisterView()
        {
            InitializeComponent();
        }

        private void password_box_PasswordChanged(object sender, RoutedEventArgs e)
        {
            if (this.DataContext != null)
            {
                ((RegisterViewModel)this.DataContext).Password = ((PasswordBox)sender).Password;
            }
        }
    }
}
