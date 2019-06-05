using System;

namespace Leopard.Helper
{
    /// <summary>
    /// 时间精度
    /// </summary>
    public enum DateTimeStampDigit
    {
        NONE = 0,

        /// <summary>
        /// 精确到 秒 。返回Stamp长度为：10 
        /// </summary>
        Second = 16,

        /// <summary>
        /// 精确到 毫秒 。返回Stamp长度为：13
        /// </summary>
        Millisecond = 32,
    }

    public static class DateTimeHelper
    {
        private static readonly DateTime UtcStartTime = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
        private static readonly DateTime LocalStartTime = TimeZone.CurrentTimeZone.ToLocalTime(UtcStartTime);

        /// <summary>
        /// 将c# DateTime时间格式转换为Unix时间戳格式(默认精度为毫秒)
        /// </summary>
        /// <param name="time">时间</param>
        /// <param name="digit">时间精度</param>
        /// <returns>Unix时间戳格式</returns> 
        public static long DateTimeToStamp(DateTime time, DateTimeStampDigit digit = DateTimeStampDigit.Millisecond)
        {
            long timeStamp = 0;

            switch (digit)
            {
                case DateTimeStampDigit.Second:
                    timeStamp = (long)(Math.Floor((DateTimeHelper.ToUniversalTime(time) - DateTimeHelper.UtcStartTime).TotalSeconds));
                    break;
                case DateTimeStampDigit.Millisecond:
                    timeStamp = (long)(Math.Floor((DateTimeHelper.ToUniversalTime(time) - DateTimeHelper.UtcStartTime).TotalMilliseconds));
                    break;
            }

            return timeStamp;
        }

        /// <summary>
        /// Unix时间戳转为本地时间   (13位)     
        /// </summary>
        /// <param name="timeStamp">Unix时间戳格式</param>
        /// <param name="digit">Unix时间戳精度</param>
        /// <returns>本地时间</returns>       
        public static DateTime StampToDateTime(long timeStamp, DateTimeStampDigit digit = DateTimeStampDigit.Millisecond)
        {
            long lTime = 0;
            switch (digit)
            {
                case DateTimeStampDigit.Second:
                    lTime = long.Parse(timeStamp + "0000000");
                    break;
                case DateTimeStampDigit.Millisecond:
                    lTime = long.Parse(timeStamp + "0000");
                    break;
            }
           
            TimeSpan toNow = new TimeSpan(lTime);
            return LocalStartTime.Add(toNow);
        }


        /// <summary>
        /// Converts a DateTime to UTC (with special handling for MinValue and MaxValue).
        /// </summary>
        /// <param name="dateTime">A DateTime.</param>
        /// <returns>The DateTime in UTC.</returns>
        public static DateTime ToUniversalTime(DateTime dateTime)
        {
            if (dateTime == DateTime.MinValue)
            {
                return DateTime.SpecifyKind(DateTime.MinValue, DateTimeKind.Utc);
            }
            else if (dateTime == DateTime.MaxValue)
            {
                return DateTime.SpecifyKind(DateTime.MaxValue, DateTimeKind.Utc);
            }
            else
            {
                return dateTime.ToUniversalTime();
            }
        }

    }
}
