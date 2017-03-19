using System;

namespace sel4.Helpers
{
    class DateHelper
    {
        static string dateFormat = "MM/dd/yyyy";
        public static string GetToday()
        {
            return DateTime.Today.ToString(dateFormat);
        }

        public static string AddDays(string date, int days)
        {
            return Convert.ToDateTime(date).AddDays(days).ToString();
        }
        public static string DaysBeforeToday(int days)
        {
            return Convert.ToDateTime(GetToday()).AddDays(-days).ToString(dateFormat);
        }
        public static string DaysAfterToday( int days)
        {
            return Convert.ToDateTime(GetToday()).AddDays(days).ToString(dateFormat);
        }

    }
}
