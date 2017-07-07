using System;

namespace Extensions.DataTypeHelpers
{
    /// <summary>Extension class to more easily parse Enums.</summary>
    public static class EnumHelper
    {
        /// <summary>Utilizes Enum.TryParse to more easily parse an Enum.</summary>
        /// <typeparam name="TEnum">Type of Enum</typeparam>
        /// <param name="text">Text to be parsed</param>
        /// <returns>Parsed Enum</returns>
        public static TEnum Parse<TEnum>(string text) where TEnum : struct
        {
            Enum.TryParse(text, true, out TEnum temp);
            return temp;
        }

        /// <summary>Returns correct Enum value if integer value is defined in the Enum, otherwise first Enum value.</summary>
        /// <typeparam name="TEnum">Type of Enum</typeparam>
        /// <param name="value">Integer to be parsed</param>
        /// <returns>Parsed Enum</returns>
        public static TEnum Parse<TEnum>(int value) where TEnum : struct
        {
            if (Enum.IsDefined(typeof(TEnum), value))
                return (TEnum)(object)value;
            return (TEnum)Enum.GetValues(typeof(TEnum)).GetValue(0);
        }
    }
}