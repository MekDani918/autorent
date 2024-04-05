using System.Collections.ObjectModel;
using System.Windows.Controls;
using System.Windows;

namespace autorent.Components
{
    public class CustomCalendar : Calendar
    {
        public ObservableCollection<CalendarDateRange> CustomBlackoutDates
        {
            get => (ObservableCollection<CalendarDateRange>)GetValue(CustomBlackoutDatesProperty);
            set
            {
                base.BlackoutDates.Clear();
                foreach (CalendarDateRange range in value)
                {
                    base.BlackoutDates.Add(range);
                }
                SetValue(CustomBlackoutDatesProperty, value);
            }
        }
        public ObservableCollection<DateTime> CustomSelectedDates
        {
            get { return (ObservableCollection<DateTime>)GetValue(CustomSelectedDatesProperty); }
            set { SetValue(CustomSelectedDatesProperty, value); }
        }

        public static readonly DependencyProperty CustomBlackoutDatesProperty =
            DependencyProperty.Register("CustomBlackoutDates", typeof(ObservableCollection<CalendarDateRange>), typeof(CustomCalendar), new PropertyMetadata(new ObservableCollection<CalendarDateRange>(), new PropertyChangedCallback(OnCustomBlackoutDatesPropertyChanged)));
        public static readonly DependencyProperty CustomSelectedDatesProperty =
            DependencyProperty.Register("CustomSelectedDates", typeof(ObservableCollection<DateTime>), typeof(CustomCalendar), new PropertyMetadata(new ObservableCollection<DateTime>(), new PropertyChangedCallback(OnCustomSelectedDatesPropertyChanged)));

        public CustomCalendar()
        {
            DisplayDateStart = DateTime.Now;
            this.SelectedDatesChanged += CustomCalendar_SelectedDatesChanged;
        }

        private static void OnCustomBlackoutDatesPropertyChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            CustomCalendar element = sender as CustomCalendar;
            if (element == null) return;

            element.BlackoutDates.Clear();

            ObservableCollection<CalendarDateRange> drangeCollection = e.NewValue as ObservableCollection<CalendarDateRange>;
            if (drangeCollection == null) return;

            foreach (CalendarDateRange drange in drangeCollection)
            {
                element.BlackoutDates.Add(drange);
            }
        }
        private static void OnCustomSelectedDatesPropertyChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            CustomCalendar element = sender as CustomCalendar;
            if (element == null) return;

            element.SelectedDates.Clear();

            ObservableCollection<DateTime> dateCollection = e.NewValue as ObservableCollection<DateTime>;
            if (dateCollection?.Count <= 0) return;

            element.SelectedDates.AddRange(dateCollection.First(), dateCollection.Last());
        }
        private void CustomCalendar_SelectedDatesChanged(object? sender, SelectionChangedEventArgs e)
        {
            if (SelectedDate != null)
                this.DisplayDate = SelectedDate.Value;
            else
                this.DisplayDate = DateTime.Now;
        }
    }
}
