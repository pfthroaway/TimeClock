using System.Globalization;

namespace ExtensionsCore.DataTypeHelpers
{
    /// <summary>Extension class to more easily parse longs.</summary>
    public static class LongHelper
    {
        /// <summary>Utilizes long.TryParse to easily parse an Integer.</summary>
        /// <param name="obj">Object to be parsed</param>
        /// <returns>Parsed integer</returns>
        public static long Parse(object value) => Parse(value.ToString());

        /// <summary>Utilizes long.TryParse to easily parse an Integer.</summary>
        /// <param name="text">Text to be parsed</param>
        /// <returns>Parsed integer</returns>
        public static long Parse(string text)
        {
            long.TryParse(text, NumberStyles.AllowThousands, CultureInfo.InvariantCulture, out long temp);
            return temp;
        }
    }
}