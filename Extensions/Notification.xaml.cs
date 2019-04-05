using Extensions.Enums;
using System;
using System.Windows;

namespace Extensions
{
    public partial class Notification
    {
        /// <summary>Toggles which buttons are displayed on the Notification.</summary>
        /// <param name="toggle">Should Yes/No buttons be enabled?</param>
        private void YesNoButtons(bool toggle)
        {
            try
            {
                BtnYes.Visibility = (Visibility)Convert.ToInt32(toggle);
                BtnNo.Visibility = (Visibility)Convert.ToInt32(toggle);
                BtnOk.Visibility = (Visibility)Convert.ToInt32(!toggle);
                ImgRed.Visibility = (Visibility)Convert.ToInt32(toggle);
                ImgYellow.Visibility = (Visibility)Convert.ToInt32(!toggle);
            }
            catch (FormatException ex)
            {
                new Notification(ex.Message, "Error Converting Boolean to Enum.", NotificationButton.OK).ShowDialog();
            }
            catch (InvalidCastException ex)
            {
                new Notification(ex.Message, "Error Converting Boolean to Enum.", NotificationButton.OK).ShowDialog();
            }
        }

        #region Button-Click Methods

        private void BtnYes_Click(object sender, RoutedEventArgs e) => CloseWindow(true);

        private void BtnOK_Click(object sender, RoutedEventArgs e) => CloseWindow(true);

        private void BtnNo_Click(object sender, RoutedEventArgs e) => CloseWindow(false);

        #endregion Button-Click Methods

        #region Window-Manipulation Methods

        /// <summary>Closes the Window.</summary>
        /// <param name="result">Result</param>
        private void CloseWindow(bool result)
        {
            DialogResult = result;
            Close();
        }

        /// <summary>Creates a new instance of Notification.</summary>
        /// <param name="text">Text to be displayed.</param>
        /// <param name="windowName">Title to be displayed on the Window.</param>
        /// <param name="buttons">Determines which buttons should be displayed on the Window</param>
        public Notification(string text, string windowName, NotificationButton buttons)
        {
            InitializeComponent();
            Title = windowName;
            TxtPopup.Text = text;
            YesNoButtons(buttons == NotificationButton.OK);
        }

        /// <summary>Creates a new instance of Notification.</summary>
        /// <param name="text">Text to be displayed.</param>
        /// <param name="windowName">Title to be displayed on the Window.</param>
        /// <param name="buttons">Determines which buttons should be displayed on the Window</param>
        /// <param name="owner">Window owner</param>
        public Notification(string text, string windowName, NotificationButton buttons, Window owner)
        {
            InitializeComponent();
            Title = windowName;
            Owner = owner;
            TxtPopup.Text = text;
            YesNoButtons(buttons == NotificationButton.OK);
        }

        #endregion Window-Manipulation Methods
    }
}