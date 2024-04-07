using System.Collections.ObjectModel;
using System.Windows.Controls;
using System.Windows;

namespace autorent.Components
{
    public class CustomDatePicker : DatePicker
    {
        public ObservableCollection<CalendarDateRange> CustomBlackoutDates
        {
            get => (ObservableCollection<CalendarDateRange>)GetValue(CustomBlackoutDatesProperty);
            set
            {
                base.BlackoutDates.Clear();
                foreach (CalendarDateRange range in value)
                {
                    try
                    {
                        base.BlackoutDates.Add(range);
                    }
                    catch { }
                }
                SetValue(CustomBlackoutDatesProperty, value);
            }
        }

        public static readonly DependencyProperty CustomBlackoutDatesProperty =
            DependencyProperty.Register(
                "CustomBlackoutDates",
                typeof(ObservableCollection<CalendarDateRange>),
                typeof(CustomDatePicker),
                new PropertyMetadata(
                    new ObservableCollection<CalendarDateRange>(),
                    new PropertyChangedCallback(OnCustomBlackoutDatesPropertyChanged)
                )
            );

        public CustomDatePicker()
        {
            DisplayDateStart = DateTime.Now;
        }

        private static void OnCustomBlackoutDatesPropertyChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            CustomDatePicker element = sender as CustomDatePicker;
            if (element == null) return;

            element.BlackoutDates.Clear();

            ObservableCollection<CalendarDateRange> drangeCollection = e.NewValue as ObservableCollection<CalendarDateRange>;
            if (drangeCollection == null) return;

            foreach (CalendarDateRange drange in drangeCollection)
            {
                try
                {
                    element.BlackoutDates.Add(drange);
                }
                catch { }
            }
        }
    }
}
