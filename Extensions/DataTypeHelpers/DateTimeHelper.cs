using System;

namespace Extensions.DataTypeHelpers
{
    /// <summary>Extension class to more easily parse DateTimes.</summary>
    public static class DateTimeHelper
    {
        /// <summary>Utilizes DateTime.TryParse to easily Parse a DateTime.</summary>
        /// <param name="text">Text to be parsed.</param>
        /// <returns>Parsed DateTime</returns>
        public static DateTime Parse(string text)
        {
            DateTime.TryParse(text, out DateTime temp);
            return temp;
        }

        /// <summary>Utilizes DateTime.TryParse to easily Parse a DateTime.</summary>
        /// <param name="obj">Object to be parsed</param>
        /// <returns>Parsed DateTime</returns>
        public static DateTime Parse(object obj)
        {
            DateTime.TryParse(obj.ToString(), out DateTime temp);
            return temp;
        }
    }
}