using System;
using System.Threading;

namespace Extensions
{
    /// <summary>Extension class which provides a more stable (and random) random number generator.</summary>
    public static class ThreadSafeRandom
    {
        [ThreadStatic] private static Random _local;

        /// <summary>Returns a Functions.GenerateRandomNumber based on this thread.</summary>
        public static Random ThisThreadsRandom => _local ??
                                                  (_local = new Random(
                                                      unchecked(Environment.TickCount * 31 +
                                                                Thread.CurrentThread.ManagedThreadId)));
    }
}