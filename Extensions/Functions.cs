using Extensions.Enums;
using Extensions.ListViewHelp;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;

namespace Extensions
{
    /// <summary>Represents a collection of useful reusable methods.</summary>
    public class Functions
    {
        /// <summary>Verifies that the requested file exists and that its file size is greater than zero. If not, it extracts the embedded file to the local output folder.</summary>
        /// <param name="resourceStream">Resource Stream from Assembly.GetExecutingAssembly().GetManifestResourceStream()</param>
        /// <param name="resourceName">Resource name</param>
        public static void VerifyFileIntegrity(Stream resourceStream, string resourceName)
        {
            FileInfo fileInfo = new FileInfo(resourceName);
            if (!File.Exists(resourceName) || fileInfo.Length == 0)
                ExtractEmbeddedResource(resourceStream, resourceName);
        }

        /// <summary>Extracts an embedded resource from a Stream.</summary>
        /// <param name="resourceStream">Resource Stream from Assembly.GetExecutingAssembly().GetManifestResourceStream()</param>
        /// <param name="resourceName">Resource name</param>
        public static void ExtractEmbeddedResource(Stream resourceStream, string resourceName)
        {
            if (resourceStream != null)
                using (BinaryReader r = new BinaryReader(resourceStream))
                using (FileStream fs =
                    new FileStream(Directory.GetCurrentDirectory() + "\\" + resourceName, FileMode.OpenOrCreate))
                using (BinaryWriter w = new BinaryWriter(fs))
                {
                    w.Write(r.ReadBytes((int)resourceStream.Length));
                }
        }

        /// <summary>Turns several Keyboard.Keys into a list of Keys which can be tested using List.Any.</summary>
        /// <param name="keys">Array of Keys</param>
        /// <returns>Returns list of Keys' IsKeyDown state</returns>
        private static IEnumerable<bool> GetListOfKeys(params Key[] keys) => keys.Select(Keyboard.IsKeyDown).ToList();

        #region Control Manipulation

        /// <summary>Adds a blank line between a TextBox's current text and the text to be added.</summary>
        /// <param name="tb">TextBox</param>
        /// <param name="newText">Text to be added</param>
        public static void AddTextToTextBox(TextBox tb, string newText)
        {
            tb.Text = string.IsNullOrWhiteSpace(tb.Text) ? newText : $"{tb.Text}\n\n{newText}";
            tb.Focus();
            tb.CaretIndex = tb.Text.Length;
            tb.ScrollToEnd();
        }

        /// <summary>Modifies a ListView to be sortable by a newly clicked column.</summary>
        /// <param name="sender">Passed column</param>
        /// <param name="sort">Class containing current column and adorner sort</param>
        /// <param name="currentListView">ListView needing modification</param>
        /// <param name="color">Color for the adorner</param>
        public static ListViewSort ListViewColumnHeaderClick(object sender, ListViewSort sort, ListView currentListView,
            string color)
        {
            Color selectedColor = Colors.Black;
            if (!string.IsNullOrWhiteSpace(color))
                selectedColor = (Color)ColorConverter.ConvertFromString(color);
            GridViewColumnHeader column = (sender as GridViewColumnHeader);
            if (column != null)
            {
                string sortBy = column.Tag.ToString();
                if (sort.Column != null)
                {
                    AdornerLayer.GetAdornerLayer(sort.Column).Remove(sort.Adorner);
                    currentListView.Items.SortDescriptions.Clear();
                }

                ListSortDirection newDir = ListSortDirection.Ascending;
                if (Equals(sort.Column, column) && sort.Adorner.Direction == newDir)
                    newDir = ListSortDirection.Descending;

                sort.Column = column;
                sort.Adorner = new SortAdorner(sort.Column, newDir, selectedColor);
                AdornerLayer.GetAdornerLayer(sort.Column).Add(sort.Adorner);
                currentListView.Items.SortDescriptions.Add(new SortDescription(sortBy, newDir));
            }

            return sort;
        }

        /// <summary>Selects all text in passed PasswordBox.</summary>
        /// <param name="sender">Object to be cast</param>
        public static void PasswordBoxGotFocus(object sender)
        {
            PasswordBox txt = (PasswordBox)sender;
            txt.SelectAll();
        }

        /// <summary>Previews a pressed key and determines whether or not it is acceptable input.</summary>
        /// <param name="e">Key being pressed</param>
        /// <param name="keyType">Type of input allowed</param>
        public static void PreviewKeyDown(KeyEventArgs e, KeyType keyType)
        {
            Key k = e.Key;

            IEnumerable<bool> keys = GetListOfKeys(Key.Back, Key.Delete, Key.Home, Key.End, Key.LeftShift,
                Key.RightShift, Key.Enter, Key.Tab, Key.LeftAlt, Key.RightAlt, Key.Left, Key.Right, Key.LeftCtrl,
                Key.RightCtrl, Key.Escape);

            switch (keyType)
            {
                case KeyType.Decimals:
                    e.Handled = !keys.Any(key => key) && (Key.D0 > k || k > Key.D9) &&
                                (Key.NumPad0 > k || k > Key.NumPad9) && k != Key.Decimal && k != Key.OemPeriod;
                    break;

                case KeyType.Integers:
                    e.Handled = !keys.Any(key => key) && (Key.D0 > k || k > Key.D9) &&
                                (Key.NumPad0 > k || k > Key.NumPad9);
                    break;

                case KeyType.Letters:
                    e.Handled = !keys.Any(key => key) && (Key.A > k || k > Key.Z) && (Key.D0 > k || k > Key.D9) &&
                                (Key.NumPad0 > k || k > Key.NumPad9);
                    break;

                case KeyType.LettersSpace:
                    e.Handled = !keys.Any(key => key) && (Key.A > k || k > Key.Z) && k != Key.Space;
                    break;

                case KeyType.LettersSpaceComma:
                    e.Handled = !keys.Any(key => key) && (Key.A > k || k > Key.Z) && k != Key.Space && k != Key.OemComma;
                    break;

                case KeyType.LettersIntegersSpace:
                    e.Handled = !keys.Any(key => key) && (Key.A > k || k > Key.Z) && (Key.D0 > k || k > Key.D9) &&
                                (Key.NumPad0 > k || k > Key.NumPad9) && k != Key.Space;
                    break;

                case KeyType.LettersIntegersSpaceComma:
                    e.Handled = !keys.Any(key => key) && (Key.A > k || k > Key.Z) && (Key.D0 > k || k > Key.D9) &&
                                (Key.NumPad0 > k || k > Key.NumPad9) && k != Key.Space && k != Key.OemComma;
                    break;

                case KeyType.NegativeDecimals:
                    e.Handled = !keys.Any(key => key) && (Key.D0 > k || k > Key.D9) &&
                                (Key.NumPad0 > k || k > Key.NumPad9) && k != Key.Decimal && k != Key.Subtract &&
                                k != Key.OemPeriod && k != Key.OemMinus;
                    break;

                case KeyType.NegativeIntegers:
                    e.Handled = !keys.Any(key => key) && (Key.D0 > k || k > Key.D9) &&
                                (Key.NumPad0 > k || k > Key.NumPad9) && k != Key.Subtract && k != Key.OemMinus;
                    break;

                default:
                    throw new ArgumentOutOfRangeException(nameof(keyType), keyType, null);
                    //&& !(Key.D0 <= k && k <= Key.D9) && !(Key.NumPad0 <= k && k <= Key.NumPad9))
                    //|| k == Key.OemMinus || k == Key.Subtract || k == Key.Decimal || k == Key.OemPeriod)
                    //System.Media.SystemSound ss = System.Media.SystemSounds.Beep;
                    //ss.Play();
            }
        }

        /// <summary>Selects all text in passed TextBox.</summary>
        /// <param name="sender">Object to be cast</param>
        public static void TextBoxGotFocus(object sender)
        {
            TextBox txt = (TextBox)sender;
            txt.SelectAll();
        }

        /// <summary>Deletes all text in textbox which isn't acceptable input.</summary>
        /// <param name="sender">Object to be cast</param>
        /// <param name="keyType">Type of input allowed</param>
        public static void TextBoxTextChanged(object sender, KeyType keyType)
        {
            TextBox txt = (TextBox)sender;
            switch (keyType)
            {
                case KeyType.Decimals:
                    txt.Text = new string((from c in txt.Text
                                           where char.IsDigit(c) || c.IsPeriod()
                                           select c).ToArray());

                    if (txt.Text.Substring(txt.Text.IndexOf(".", StringComparison.Ordinal) + 1).Contains("."))
                        txt.Text = txt.Text.Substring(0, txt.Text.IndexOf(".", StringComparison.Ordinal) + 1) + txt.Text
                                       .Substring(txt.Text.IndexOf(".", StringComparison.Ordinal) + 1).Replace(".", "");
                    break;

                case KeyType.Integers:
                    txt.Text = new string((from c in txt.Text
                                           where char.IsDigit(c)
                                           select c).ToArray());
                    break;

                case KeyType.Letters:
                    txt.Text = new string((from c in txt.Text
                                           where char.IsLetter(c)
                                           select c).ToArray());
                    break;

                case KeyType.LettersSpace:
                    txt.Text = new string((from c in txt.Text
                                           where char.IsLetter(c) || c.IsSpace()
                                           select c).ToArray());
                    break;

                case KeyType.LettersSpaceComma:
                    txt.Text = new string((from c in txt.Text
                                           where char.IsLetter(c) || c.IsSpace() || c.IsComma()
                                           select c).ToArray());
                    break;

                case KeyType.LettersIntegersSpace:
                    txt.Text = new string((from c in txt.Text
                                           where char.IsLetterOrDigit(c) || c.IsSpace()
                                           select c).ToArray());
                    break;

                case KeyType.LettersIntegersSpaceComma:
                    txt.Text = new string((from c in txt.Text
                                           where char.IsLetterOrDigit(c) || c.IsSpace() || c.IsComma()
                                           select c).ToArray());
                    break;

                case KeyType.NegativeDecimals:
                    txt.Text = new string((from c in txt.Text
                                           where char.IsDigit(c) || c.IsPeriod() || c.IsHyphen()
                                           select c).ToArray());

                    if (txt.Text.Substring(txt.Text.IndexOf(".", StringComparison.Ordinal) + 1).Contains("."))
                        txt.Text = txt.Text.Substring(0, txt.Text.IndexOf(".", StringComparison.Ordinal) + 1) + txt.Text
                                       .Substring(txt.Text.IndexOf(".", StringComparison.Ordinal) + 1).Replace(".", "");

                    if (txt.Text.Substring(txt.Text.IndexOf("-", StringComparison.Ordinal) + 1).Contains("-"))
                        txt.Text = txt.Text.Substring(0, txt.Text.IndexOf("-", StringComparison.Ordinal) + 1) + txt.Text
                                       .Substring(txt.Text.IndexOf("-", StringComparison.Ordinal) + 1).Replace("-", "");
                    break;

                case KeyType.NegativeIntegers:
                    txt.Text = new string((from c in txt.Text
                                           where char.IsDigit(c) || c.IsHyphen()
                                           select c).ToArray());

                    if (txt.Text.Substring(txt.Text.IndexOf("-", StringComparison.Ordinal) + 1).Contains("-"))
                        txt.Text = txt.Text.Substring(0, txt.Text.IndexOf("-", StringComparison.Ordinal) + 1) + txt.Text
                                       .Substring(txt.Text.IndexOf("-", StringComparison.Ordinal) + 1).Replace("-", "");
                    break;

                default:
                    throw new ArgumentOutOfRangeException(nameof(keyType), keyType, null);
            }
        }

        #endregion Control Manipulation

        #region Random Number Generation

        /// <summary>Generates a random number between min and max (inclusive).</summary>
        /// <param name="min">Inclusive minimum number</param>
        /// <param name="max">Inclusive maximum number</param>
        /// <param name="lowerLimit">Minimum limit for the method, regardless of min and max.</param>
        /// <param name="upperLimit">Maximum limit for the method, regardless of min and max.</param>
        /// <returns>Returns randomly generated integer between min and max with an upper limit of upperLimit.</returns>
        public static int GenerateRandomNumber(int min, int max, int lowerLimit = int.MinValue,
            int upperLimit = int.MaxValue)
        {
            if (min < lowerLimit)
                min = lowerLimit;
            if (max > upperLimit)
                max = upperLimit;
            int result = min < max
                ? ThreadSafeRandom.ThisThreadsRandom.Next(min, max + 1)
                : ThreadSafeRandom.ThisThreadsRandom.Next(max, min + 1);

            return result;
        }

        #endregion Random Number Generation
    }
}