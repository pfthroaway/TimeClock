using System;
using System.Windows;
using TimeClock.Classes;
using TimeClock.Pages.Admin;

namespace TimeClock.Pages
{
    /// <summary>Interaction logic for MainWindow.xaml</summary>
    public partial class MainWindow
    {
        #region ScaleValue Depdency Property

        public static readonly DependencyProperty ScaleValueProperty = DependencyProperty.Register("ScaleValue", typeof(double), typeof(MainWindow), new UIPropertyMetadata(1.0, new PropertyChangedCallback(OnScaleValueChanged), new CoerceValueCallback(OnCoerceScaleValue)));

        private static object OnCoerceScaleValue(DependencyObject o, object value)
        {
            MainWindow mainPage = o as MainWindow;
            return mainPage?.OnCoerceScaleValue((double)value) ?? value;
        }

        private static void OnScaleValueChanged(DependencyObject o, DependencyPropertyChangedEventArgs e)
        {
            MainWindow mainPage = o as MainWindow;
            mainPage?.OnScaleValueChanged((double)e.OldValue, (double)e.NewValue);
        }

        protected virtual double OnCoerceScaleValue(double value)
        {
            if (double.IsNaN(value))
                return 1.0f;

            value = Math.Max(0.1, value);
            return value;
        }

        protected virtual void OnScaleValueChanged(double oldValue, double newValue)
        {
        }

        public double ScaleValue
        {
            get => (double)GetValue(ScaleValueProperty);
            set => SetValue(ScaleValueProperty, value);
        }

        #endregion ScaleValue Depdency Property

        /// <summary>Calculates the scale for the Page.</summary>
        internal void CalculateScale()
        {
            double yScale = ActualHeight / AppState.CurrentPageHeight;
            double xScale = ActualWidth / AppState.CurrentPageWidth;
            double value = Math.Min(xScale, yScale) * 0.8;
            if (value > 3)
                value = 3;
            else if (value < 1)
                value = 1;
            ScaleValue = (double)OnCoerceScaleValue(WindowMain, value);
        }

        #region Click Methods

        private void MnuAdmin_Click(object sender, RoutedEventArgs e)
        {
            MainFrame.Navigate(new AdminPasswordPage());
            MnuAdmin.IsEnabled = false;
        }

        private void MnuFileExit_Click(object sender, RoutedEventArgs e) => Close();

        #endregion Click Methods

        #region Page-Manipulation Methods

        public MainWindow()
        {
            InitializeComponent();
            AppState.MainWindow = this;
        }

        private async void WindowMain_Loaded(object sender, RoutedEventArgs e) => await AppState.LoadAll();

        private void MainFrame_OnSizeChanged(object sender, SizeChangedEventArgs e) => CalculateScale();

        #endregion Page-Manipulation Methods
    }
}