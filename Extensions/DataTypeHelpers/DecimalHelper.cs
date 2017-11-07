using System.Globalization;

namespace Extensions.DataTypeHelpers
{
    /// <summary>Extension class to more easily parse Decimals.</summary>
    public static class DecimalHelper
    {
        /// <summary>Utilizes decimal.TryParse to easily parse a Decimal.</summary>
        /// <param name="text">Text to be parsed</param>
        /// <returns>Parsed Decimal</returns>
        public static decimal Parse(string text)
        {
            decimal.TryParse(text, NumberStyles.AllowThousands, CultureInfo.InvariantCulture, out decimal temp);
            return temp;
        }

        /// <summary>Utilizes decimal.TryParse to easily parse a Decimal.</summary>
        /// <param name="obj">Object to be parsed</param>
        /// <returns>Parsed Decimal</returns>
        public static decimal Parse(object obj)
        {
            decimal.TryParse(obj.ToString(), NumberStyles.AllowThousands, CultureInfo.InvariantCulture, out decimal temp);
            return temp;
        }
    }
}