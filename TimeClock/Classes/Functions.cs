using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Controls;
using System.Windows.Input;

namespace TimeClock
{
    internal enum KeyType { DecimalNumbers, Letters, NegativeDecimalNumbers, NegativeNumbers, Numbers }

    internal static class Functions
    {
        /// <summary>Turns several Keyboard.Keys into a list of Keys which can be tested using List.Any.</summary>
        /// <param name="keys">Array of Keys</param>
        /// <returns>Returns list of Keys' IsKeyDown state</returns>
        private static IEnumerable<bool> GetListOfKeys(params Key[] keys)
        {
            return keys.Select(Keyboard.IsKeyDown).ToList();
        }

        /// <summary>Selects all text in passed TextBox.</summary>
        /// <param name="sender">Object to be cast</param>
        internal static void TextBoxGotFocus(object sender)
        {
            TextBox txt = (TextBox)sender;
            txt.SelectAll();
        }

        /// <summary>Selects all text in passed PasswordBox.</summary>
        /// <param name="sender">Object to be cast</param>
        internal static void PasswordBoxGotFocus(object sender)
        {
            PasswordBox txt = (PasswordBox)sender;
            txt.SelectAll();
        }

        /// <summary>Deletes all text in textbox which isn't a letter.</summary>
        /// <param name="sender">Object to be cast</param>
        /// <param name="keyType">Type of input allowed</param>
        internal static void TextBoxTextChanged(object sender, KeyType keyType)
        {
            TextBox txt = (TextBox)sender;
            switch (keyType)
            {
                case KeyType.DecimalNumbers:
                    break;

                case KeyType.Letters:
                    txt.Text = new string((from c in txt.Text
                                           where char.IsLetter(c)
                                           select c).ToArray());
                    break;

                case KeyType.NegativeDecimalNumbers:
                    break;

                case KeyType.NegativeNumbers:
                    break;

                case KeyType.Numbers:
                    txt.Text = new string((from c in txt.Text
                                           where char.IsDigit(c)
                                           select c).ToArray());
                    break;

                default:
                    throw new ArgumentOutOfRangeException(nameof(keyType), keyType, null);
            }
            txt.CaretIndex = txt.Text.Length;
        }

        /// <summary>Previews a pressed key and determines whether or not it is acceptable input.</summary>
        /// <param name="e">Key being pressed</param>
        /// <param name="keyType">Type of input allowed</param>
        internal static void PreviewKeyDown(KeyEventArgs e, KeyType keyType)
        {
            Key k = e.Key;

            IEnumerable<bool> keys = GetListOfKeys(Key.Back, Key.Delete, Key.Home, Key.End, Key.LeftShift, Key.RightShift, Key.Enter, Key.Tab, Key.LeftAlt, Key.RightAlt, Key.Left, Key.Right, Key.LeftCtrl, Key.RightCtrl, Key.Escape);

            switch (keyType)
            {
                case KeyType.DecimalNumbers:
                    break;

                case KeyType.Letters:
                    e.Handled = !keys.Any(key => key) && (Key.A > k || k > Key.Z);
                    break;

                case KeyType.NegativeDecimalNumbers:
                    break;

                case KeyType.NegativeNumbers:
                    break;

                case KeyType.Numbers:
                    e.Handled = !keys.Any(key => key) && (Key.D0 > k || k > Key.D9) && (Key.NumPad0 > k || k > Key.NumPad9);
                    break;

                default:
                    throw new ArgumentOutOfRangeException(nameof(keyType), keyType, null);
                    //&& !(Key.D0 <= k && k <= Key.D9) && !(Key.NumPad0 <= k && k <= Key.NumPad9))
                    //|| k == Key.OemMinus || k == Key.Subtract || k == Key.Decimal || k == Key.OemPeriod)
                    //System.Media.SystemSound ss = System.Media.SystemSounds.Beep;
                    //ss.Play();
            }
        }
    }
}