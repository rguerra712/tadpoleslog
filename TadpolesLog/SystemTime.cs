using System;

namespace TadpolesLog
{
    public static class SystemTime
    {
        public static Func<DateTime> Now { get; set; } = () => DateTime.Now;
    }
}