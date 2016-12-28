using System;
using System.Windows;

namespace TimeClock
{
    public enum NotificationButtons { YesNo, OK }

    public partial class Notification
    {
        /// <summary>Toggles which buttons are displayed on the Notification.</summary>
        /// <param name="toggle">Should Yes/No buttons be enabled?</param>
        private void YesNoButtons(bool toggle)
        {
            try
            {
                btnYes.Visibility = (Visibility)Convert.ToInt32(toggle);
                btnNo.Visibility = (Visibility)Convert.ToInt32(toggle);
                btnOK.Visibility = (Visibility)Convert.ToInt32(!toggle);
                imgRed.Visibility = (Visibility)Convert.ToInt32(toggle);
                imgYellow.Visibility = (Visibility)Convert.ToInt32(!toggle);
            }
            catch (Exception ex)
            {
                new Notification(ex.Message, "Error Converting Boolean to Enum.", NotificationButtons.OK, this).ShowDialog();
            }
        }

        #region Button-Click Methods

        private void btnYes_Click(object sender, RoutedEventArgs e)
        {
            CloseWindow(true);
        }

        private void btnOK_Click(object sender, RoutedEventArgs e)
        {
            CloseWindow(true);
        }

        private void btnNo_Click(object sender, RoutedEventArgs e)
        {
            CloseWindow(false);
        }

        #endregion Button-Click Methods

        #region Window-Manipulation Methods

        /// <summary>Closes the Window.</summary>
        private void CloseWindow(bool result)
        {
            DialogResult = result;
            this.Close();
        }

        /// <summary>Creates a new instance of Notification.</summary>
        /// <param name="text">Text to be displayed.</param>
        /// <param name="windowName">Title to be displayed on the Window.</param>
        /// <param name="buttons">Determines which buttons should be displayed on the Window</param>
        public Notification(string text, string windowName, NotificationButtons buttons)
        {
            InitializeComponent();
            Title = windowName;
            txtPopup.Text = text;
            YesNoButtons(buttons == NotificationButtons.OK);
        }

        /// <summary>Creates a new instance of Notification.</summary>
        /// <param name="text">Text to be displayed.</param>
        /// <param name="windowName">Title to be displayed on the Window.</param>
        /// <param name="buttons">Determines which buttons should be displayed on the Window</param>
        /// <param name="owner">Window owner</param>
        public Notification(string text, string windowName, NotificationButtons buttons, Window owner)
        {
            InitializeComponent();
            Title = windowName;
            Owner = owner;
            txtPopup.Text = text;
            YesNoButtons(buttons == NotificationButtons.OK);
        }

        #endregion Window-Manipulation Methods
    }
}