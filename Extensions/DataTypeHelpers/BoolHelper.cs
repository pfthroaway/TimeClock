using Extensions.Enums;
using System;

namespace Extensions.DataTypeHelpers
{
    /// <summary>Extension class to more easily parse Booleans.</summary>
    public static class BoolHelper
    {
        /// <summary>Utilizes bool.TryParse to easily parse a Boolean.</summary>
        /// <param name="text">Text to be parsed</param>
        /// <returns>Parsed Boolean</returns>
        public static bool Parse(string text)
        {
            bool.TryParse(text, out bool temp);
            return temp;
        }

        /// <summary>Utilizes Convert.ToBoolean to easily parse a Boolean.</summary>
        /// <param name="obj">Object to be parsed</param>
        /// <returns>Parsed Boolean</returns>
        public static bool Parse(object obj)
        {
            bool temp = false;
            try
            {
                temp = Convert.ToBoolean(obj);
            }
            catch (Exception ex)
            {
                new Notification(ex.Message, "Error Parsing Boolean", NotificationButtons.OK).ShowDialog();
            }
            return temp;
        }

        /// <summary>Converts an Integer to Boolean.</summary>
        /// <param name="num">Integer to be converted</param>
        /// <returns>Converted Boolean</returns>
        public static bool Parse(int num) => num != 0;
    }
}