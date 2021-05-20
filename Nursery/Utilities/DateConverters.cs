using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Nursery.Utilities
{
    public static class DateConverters
    {
        public static string ToRelativeDate(this DateTime date)
        {
            var dist = DateTime.Now.Subtract(date);
            string ans = "";

            if (TimeSpan.FromSeconds(0) <= dist && dist > TimeSpan.FromSeconds(60))
            {
                ans = $"{dist.Seconds} ثانیه";
            }
            else if (TimeSpan.FromMinutes(1) <= dist && dist > TimeSpan.FromMinutes(60))
            {
                ans = $"{dist.Minutes} دقیقه";
            }
            else if (TimeSpan.FromHours(1) <= dist && dist > TimeSpan.FromHours(24))
            {
                ans = $"{dist.Hours} ساعت";
            }
            else if (TimeSpan.FromDays(1) <= dist && dist > TimeSpan.FromDays(365))
            {
                ans = $"{dist.Days} روز";
            }
            else if (TimeSpan.FromDays(365) <= dist)
            {
                ans = $"{Math.Round((double)dist.Days / 365)} سال";
            }

            return ans;
        }

    }
}
