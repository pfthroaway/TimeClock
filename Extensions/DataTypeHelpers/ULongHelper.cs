using System.Globalization;

namespace ExtensionsCore.DataTypeHelpers
{
    /// <summary>Extension class to more easily parse ulongs.</summary>
    public static class ULongHelper
    {
        /// <summary>Utilizes ulong.TryParse to easily parse an Integer.</summary>
        /// <param name="obj">Object to be parsed</param>
        /// <returns>Parsed integer</returns>
        public static ulong Parse(object value) => Parse(value.ToString());

        /// <summary>Utilizes ulong.TryParse to easily parse an Integer.</summary>
        /// <param name="text">Text to be parsed</param>
        /// <returns>Parsed integer</returns>
        public static ulong Parse(string text)
        {
            ulong.TryParse(text, NumberStyles.AllowThousands, CultureInfo.InvariantCulture, out ulong temp);
            return temp;
        }
    }
}