using System;

namespace TadpolesLog.Extensions
{
    public static class DateTimeExtensions
    {
        public static long ToEpochTime(this DateTime dateTime)
        {
            dateTime = dateTime.ToUniversalTime();
            return (long)(dateTime - new DateTime(1970, 1, 1)).TotalSeconds;
        }
    }
}