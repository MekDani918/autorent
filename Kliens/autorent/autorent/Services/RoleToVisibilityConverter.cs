using System.Globalization;
using System.Windows.Data;
using System.Windows;
using System.Diagnostics;

namespace autorent.Services
{
    public class RoleToVisibilityConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            //Debug.WriteLine(value.ToString());
           // Debug.WriteLine(parameter.ToString());

            if (value.ToString() == parameter.ToString())
                return Visibility.Visible;

            return Visibility.Collapsed;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return "";
        }
    }
}