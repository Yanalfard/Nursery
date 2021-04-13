using DataLayer.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace DataLayer.ViewModels
{
    public class TimelineVm
    {
        public int UserId { get; set; }
        public TblRole Role { get; set; }
        /// <summary>
        /// Max 1440
        /// </summary>
        public int MinuteStart { get; set; }
        /// <summary>
        /// Max 1440
        /// </summary>
        public int MinuteEnd { get; set; }

        public TimelineVm()
        {
            UserId = 0;
            Role = new TblRole() { Name = "ROLE" };
            MinuteStart = (int)Math.Round(DateTime.Now.TimeOfDay.TotalMinutes);
            MinuteEnd = (int)Math.Round(DateTime.Now.AddMinutes(5).TimeOfDay.TotalMinutes);
        }
    }
}
