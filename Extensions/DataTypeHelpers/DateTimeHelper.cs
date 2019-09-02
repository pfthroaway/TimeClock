using System;

namespace Extensions.DataTypeHelpers
{
    /// <summary>Extension class to more easily parse DateTimes.</summary>
    public static class DateTimeHelper
    {
        /// <summary>Utilizes DateTime.TryParse to easily parse a DateTime.</summary>
        /// <param name="obj">Object to be parsed</param>
        /// <returns>Parsed DateTime</returns>
        public static DateTime Parse(object value) => value != null ? Parse(value.ToString()) : DateTime.MinValue;

        /// <summary>Utilizes DateTime.TryParse to easily parse a DateTime.</summary>
        /// <param name="text">Text to be parsed.</param>
        /// <returns>Parsed DateTime</returns>
        public static DateTime Parse(string text) => string.IsNullOrWhiteSpace(text) ? DateTime.MinValue : DateTime.TryParse(text, out DateTime temp) ? temp : DateTime.MinValue;
    }
}