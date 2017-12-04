using System;

namespace Rakon.Test.Core
{
    public static class SystemTime
    {
        private static readonly DateTime TheBeginningOfTime = DateTime.Now;
        private const double SpeedFactor = 60; // 1 second -> 60 seconds (1 minute)

        public static DateTime Now => GetTime();

        private static DateTime GetTime()
        {
            var timeDelta = DateTime.Now - TheBeginningOfTime;
            var timeOffset = TimeSpan.FromSeconds(timeDelta.TotalSeconds * SpeedFactor);
            return TheBeginningOfTime.Add(timeOffset);
        }
    }
}