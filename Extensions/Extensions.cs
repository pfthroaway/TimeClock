using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;

namespace Extensions
{
    /// <summary>Extension class which provides a more stable (and random) random number generator.</summary>
    public static class ThreadSafeRandom
    {
        [ThreadStatic]
        private static Random _local;

        /// <summary>Returns a Random based on this thread.</summary>
        public static Random ThisThreadsRandom => _local ??
                                                    (_local = new Random(unchecked(Environment.TickCount * 31 + Thread.CurrentThread.ManagedThreadId)));
    }

    /// <summary>Extension class to more easily parse Decimals.</summary>
    public static class DecimalHelper
    {
        /// <summary>Utilizes decimal.TryParse to easily Parse a Decimal.</summary>
        /// <param name="text">Text to be parsed</param>
        /// <returns>Parsed Decimal</returns>
        public static decimal Parse(string text)
        {
            decimal.TryParse(text, out decimal temp);
            return temp;
        }

        /// <summary>Utilizes decimal.TryParse to easily Parse a Decimal.</summary>
        /// <param name="obj">Object to be parsed</param>
        /// <returns>Parsed Decimal</returns>
        public static decimal Parse(object obj)
        {
            decimal.TryParse(obj.ToString(), out decimal temp);
            return temp;
        }
    }

    /// <summary>Extension class to more easily parse Integers.</summary>
    public static class Int32Helper
    {
        /// <summary>Utilizes int.TryParse to easily Parse an Integer.</summary>
        /// <param name="text">Text to be parsed</param>
        /// <returns></returns>
        public static int Parse(string text)
        {
            int.TryParse(text, out int temp);
            return temp;
        }

        /// <summary>Utilizes int.TryParse to easily Parse an Integer.</summary>
        /// <param name="dbl">Double to be parsed</param>
        /// <returns>Parsed integer</returns>
        public static int Parse(double dbl)
        {
            int temp = 0;
            try
            {
                temp = (int)dbl;
            }
            catch (Exception e)
            {
                new Notification(e.Message, "Error Parsing Integer", NotificationButtons.OK).ShowDialog();
            }
            return temp;
        }

        /// <summary>Utilizes int.TryParse to easily Parse an Integer.</summary>
        /// <param name="dcml">Decimal to be parsed</param>
        /// <returns>Parsed integer</returns>
        public static int Parse(decimal dcml)
        {
            int temp = 0;
            try
            {
                temp = (int)dcml;
            }
            catch (Exception e)
            {
                new Notification(e.Message, "Error Parsing Integer", NotificationButtons.OK).ShowDialog();
            }

            return temp;
        }

        /// <summary>Utilizes int.TryParse to easily Parse an Integer.</summary>
        /// <param name="obj">Object to be parsed</param>
        /// <returns>Parsed integer</returns>
        public static int Parse(object obj)
        {
            int.TryParse(obj.ToString(), out int temp);
            return temp;
        }

        public static int Parse(bool bln)
        {
            int temp = 0;
            try
            {
                temp = Convert.ToInt32(bln);
            }
            catch (Exception ex)
            {
                new Notification(ex.Message, "Error Converting Boolean to Integer", NotificationButtons.OK).ShowDialog();
            }
            return temp;
        }
    }

    /// <summary>Extension class to more easily parse Booleans.</summary>
    public static class BoolHelper
    {
        /// <summary>Utilizes bool.TryParse to easily Parse a Boolean.</summary>
        /// <param name="text">Text to be parsed</param>
        /// <returns>Parsed Boolean</returns>
        public static bool Parse(string text)
        {
            bool.TryParse(text, out bool temp);
            return temp;
        }

        /// <summary>Utilizes Convert.ToBoolean to easily Parse a Boolean.</summary>
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
    }

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

    public static class MyExtensions
    {/// <summary>Adds multiple ranges to a List.</summary>
     /// <typeparam name="T">Type</typeparam>
     /// <param name="list">List for others to be added to</param>
     /// <param name="lists">Lists to be added to list</param>
        public static void AddRanges<T>(this List<T> list, params IEnumerable<T>[] lists)
        {
            foreach (IEnumerable<T> currentList in lists)
                list.AddRange(currentList);
        }

        /// <summary>Determines if this character is a hyphen.</summary>
        /// <param name="c">Character to be evaluated</param>
        /// <returns>Returns true if character is a hyphen</returns>
        public static bool IsHyphen(this char c)
        {
            return c.Equals('-');
        }

        /// <summary>Determines if this character is a period.</summary>
        /// <param name="c">Character to be evaluated</param>
        /// <returns>Returns true if character is a period</returns>
        public static bool IsPeriod(this char c)
        {
            return c.Equals('.');
        }

        /// <summary>Determines if this character is a period or comma.</summary>
        /// <param name="c">Character to be evaluated</param>
        /// <returns>Returns true if character is a period or comma</returns>
        public static bool IsPeriodOrComma(this char c)
        {
            return c.Equals('.') | c.Equals(',');
        }

        /// <summary>Shuffles a List.</summary>
        /// <typeparam name="T">Generic Type</typeparam>
        /// <param name="list">List Name</param>
        public static void Shuffle<T>(this IList<T> list)
        {
            int n = list.Count;
            while (n > 1)
            {
                n--;
                int k = ThreadSafeRandom.ThisThreadsRandom.Next(n + 1);
                T value = list[k];
                list[k] = list[n];
                list[n] = value;
            }
        }

        /// <summary>Waits for an asynchronous Process to exit asynchronously.</summary>
        /// <param name="process">Process to be awaited</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>Task</returns>
        public static Task WaitForExitAsync(this Process process, CancellationToken cancellationToken = default(CancellationToken))
        {
            TaskCompletionSource<object> tcs = new TaskCompletionSource<object>();
            process.EnableRaisingEvents = true;
            process.Exited += (sender, args) => tcs.TrySetResult(null);
            if (cancellationToken != default(CancellationToken))
                cancellationToken.Register(tcs.SetCanceled);

            return tcs.Task;
        }
    }
}