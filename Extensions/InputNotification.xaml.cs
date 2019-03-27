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
using System.Windows.Shapes;

namespace Extensions
{
    /// <summary>Interaction logic for InputNotification.xaml</summary>
    public partial class InputNotification : Window
    {
        #region Button Click Methods

        private void BtnSubmit_Click(object sender, RoutedEventArgs e) => CloseWindow(true);

        private void BtnExit_Click(object sender, RoutedEventArgs e) => CloseWindow(false);

        #endregion Button Click Methods

        #region Input Manipulation

        private void TxtInput_TextChanged(object sender, TextChangedEventArgs e) => BtnSubmit.IsEnabled = TxtInput.Text.Length > 0;

        #endregion Input Manipulation

        #region Window Manipulation

        /// <summary>Closes the Window.</summary>
        /// <param name="result">Result</param>
        private void CloseWindow(bool result)
        {
            DialogResult = result;
            Close();
        }

        /// <summary>Creates a new instance of <see cref="InputNotification"/>.</summary>
        /// <param name="text">Text to be displayed.</param>
        /// <param name="windowName">Title to be displayed on the Window.</param>
        /// <param name="defaultText">Text to be displayed in the TxtInput TextBox by default.</param>
        public InputNotification(string text, string windowName, string defaultText = "")
        {
            InitializeComponent();
            Title = windowName;
            TxtPopup.Text = text;
            TxtInput.Text = defaultText;
            TxtInput.Focus();
        }

        /// <summary>Creates a new instance of <see cref="InputNotification"/>.</summary>
        /// <param name="text">Text to be displayed.</param>
        /// <param name="windowName">Title to be displayed on the Window.</param>
        /// <param name="owner">Window owner</param>
        /// <param name="defaultText">Text to be displayed in the TxtInput TextBox by default.</param>
        public InputNotification(string text, string windowName, Window owner, string defaultText = "")
        {
            InitializeComponent();
            Title = windowName;
            Owner = owner;
            TxtPopup.Text = text;
            TxtInput.Text = defaultText;
            TxtInput.Focus();
        }

        #endregion Window Manipulation
    }
}