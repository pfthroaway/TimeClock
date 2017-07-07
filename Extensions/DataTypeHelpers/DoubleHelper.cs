namespace Extensions.DataTypeHelpers
{
    /// <summary>Extension class to more easily parse Doubles.</summary>
    public static class DoubleHelper
    {
        /// <summary>Utilizes double.TryParse to easily Parse a Double.</summary>
        /// <param name="text">Text to be parsed</param>
        /// <returns>Parsed Double</returns>
        public static double Parse(string text)
        {
            double.TryParse(text, out double temp);
            return temp;
        }

        /// <summary>Utilizes double.TryParse to easily Parse a Double.</summary>
        /// <param name="obj">Object to be parsed</param>
        /// <returns>Parsed Double</returns>
        public static double Parse(object obj)
        {
            double.TryParse(obj.ToString(), out double temp);
            return temp;
        }
    }
}