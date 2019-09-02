using Extensions.Enums;
using System;
using System.Globalization;

namespace Extensions.DataTypeHelpers
{
    /// <summary>Extension class to more easily parse Integers.</summary>
    public static class Int32Helper
    {
        /// <summary>Converts a boolean to integer.</summary>
        /// <param name="bln">Boolean to be parsed</param>
        /// <returns>Parsed integer</returns>
        public static int Parse(bool bln) => bln ? 1 : 0;

        /// <summary>Attempts to easily parse an Integer.</summary>
        /// <param name="dcml">Decimal to be parsed</param>
        /// <returns>Parsed integer</returns>
        public static int Parse(decimal dcml)
        {
            int temp = 0;
            try
            {
                temp = (int)dcml;
            }
            catch (InvalidCastException ex)
            {
                new Notification(ex.Message, "Error Parsing Integer", NotificationButton.OK).ShowDialog();
            }

            return temp;
        }

        /// <summary>Attempts to easily parse an Integer.</summary>
        /// <param name="dbl">Double to be parsed</param>
        /// <returns>Parsed integer</returns>
        public static int Parse(double dbl)
        {
            int temp = 0;
            try
            {
                temp = (int)dbl;
            }
            catch (InvalidCastException ex)
            {
                new Notification(ex.Message, "Error Parsing Integer", NotificationButton.OK).ShowDialog();
            }
            return temp;
        }

        /// <summary>Utilizes Convert.ToInt32 to easily parse an Integer.</summary>
        /// <param name="enumer">Enum to be parsed</param>
        /// <returns>Parsed integer</returns>
        public static int Parse(Enum enumer)
        {
            int temp = 0;
            try
            {
                temp = Convert.ToInt32(enumer, new CultureInfo("en-US"));
            }
            catch (FormatException ex)
            {
                new Notification(ex.Message, "Error Parsing Integer", NotificationButton.OK).ShowDialog();
            }
            catch (InvalidCastException ex)
            {
                new Notification(ex.Message, "Error Parsing Integer", NotificationButton.OK).ShowDialog();
            }
            return temp;
        }

        /// <summary>Utilizes int.TryParse to easily parse an Integer.</summary>
        /// <param name="obj">Object to be parsed</param>
        /// <returns>Parsed integer</returns>
        public static int Parse(object value) => Parse(value.ToString());

        /// <summary>Utilizes int.TryParse to easily parse an Integer.</summary>
        /// <param name="text">Text to be parsed</param>
        /// <returns>Parsed integer</returns>
        public static int Parse(string text)
        {
            int.TryParse(text, NumberStyles.AllowThousands, CultureInfo.InvariantCulture, out int temp);
            return temp;
        }
    }
}