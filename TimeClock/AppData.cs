using System;
using System.IO;

namespace TimeClock
{
    public static class AppData
    {
        internal static string Location = Path.Combine(
               Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "TimeClock");
    }
}