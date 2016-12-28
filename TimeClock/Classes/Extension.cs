using System;

namespace TimeClock
{
    /// <summary>Extension class to more easily parse Integers.</summary>
    internal static class Int32Helper
    {
        /// <summary>Utilizes int.TryParse to easily Parse an Integer.</summary>
        /// <param name="text">Text to be parsed</param>
        /// <returns></returns>
        internal static int Parse(string text)
        {
            int temp;
            int.TryParse(text, out temp);
            return temp;
        }

        /// <summary>Utilizes int.TryParse to easily Parse an Integer.</summary>
        /// <param name="dbl">Double to be parsed</param>
        /// <returns>Parsed integer</returns>
        internal static int Parse(double dbl)
        {
            int temp = 0;
            try
            {
                temp = (int)dbl;
            }
            catch (Exception e)
            { new Notification(e.Message, "Sulimn", NotificationButtons.OK).ShowDialog(); }

            return temp;
        }

        /// <summary>Utilizes int.TryParse to easily Parse an Integer.</summary>
        /// <param name="dcml">Decimal to be parsed</param>
        /// <returns>Parsed integer</returns>
        internal static int Parse(decimal dcml)
        {
            int temp = 0;
            try
            {
                temp = (int)dcml;
            }
            catch (Exception e)
            { new Notification(e.Message, "Sulimn", NotificationButtons.OK).ShowDialog(); }

            return temp;
        }

        /// <summary>Utilizes int.TryParse to easily Parse an Integer.</summary>
        /// <param name="obj">Object to be parsed</param>
        /// <returns>Parsed integer</returns>
        internal static int Parse(object obj)
        {
            int temp;
            int.TryParse(obj.ToString(), out temp);
            return temp;
        }
    }

    /// <summary>Extension class to more easily parse Booleans.</summary>
    internal static class BoolHelper
    {
        /// <summary>Utilizes bool.TryParse to easily Parse a Boolean.</summary>
        /// <param name="text">Text to be parsed</param>
        /// <returns>Parsed Boolean</returns>
        internal static bool Parse(string text)
        {
            bool temp;
            bool.TryParse(text, out temp);
            return temp;
        }

        /// <summary>Utilizes Convert.ToBoolean to easily Parse a Boolean.</summary>
        /// <param name="obj">Object to be parsed</param>
        /// <returns>Parsed Boolean</returns>
        internal static bool Parse(object obj)
        {
            bool temp = false;
            try
            {
                temp = Convert.ToBoolean(obj);
            }
            catch (Exception ex)
            {
                new Notification(ex.Message, "Assassin", NotificationButtons.OK).ShowDialog();
            }
            return temp;
        }
    }

    /// <summary>Extension class to more easily parse DateTimes.</summary>
    internal static class DateTimeHelper
    {
        /// <summary>Utilizes DateTime.TryParse to easily Parse a DateTime.</summary>
        /// <param name="text">Text to be parsed.</param>
        /// <returns>Parsed DateTime</returns>
        internal static DateTime Parse(string text)
        {
            DateTime temp;
            DateTime.TryParse(text, out temp);
            return temp;
        }

        /// <summary>Utilizes DateTime.TryParse to easily Parse a DateTime.</summary>
        /// <param name="obj">Object to be parsed</param>
        /// <returns>Parsed DateTime</returns>
        internal static DateTime Parse(object obj)
        {
            DateTime temp;
            DateTime.TryParse(obj.ToString(), out temp);
            return temp;
        }
    }

    /// <summary>Extension class to more easily parse Doubles.</summary>
    internal static class DoubleHelper
    {
        /// <summary>Utilizes double.TryParse to easily Parse a Double.</summary>
        /// <param name="text">Text to be parsed</param>
        /// <returns>Parsed Double</returns>
        internal static double Parse(string text)
        {
            double temp;
            double.TryParse(text, out temp);
            return temp;
        }

        /// <summary>Utilizes double.TryParse to easily Parse a Double.</summary>
        /// <param name="obj">Object to be parsed</param>
        /// <returns>Parsed Double</returns>
        internal static double Parse(object obj)
        {
            double temp;
            double.TryParse(obj.ToString(), out temp);
            return temp;
        }
    }
}