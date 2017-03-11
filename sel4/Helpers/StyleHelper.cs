
using System.Collections.Generic;
using System.Linq;



namespace sel4.Helpers
{


    public static class StyleHelper
    {
        private static List<string> Red = new List<string>() { "rgb(204, 0, 0)", "b" , "rgba(204, 0, 0, 1)" };

        private static List<string> Grey = new List<string>() { "rgb(119, 119, 119)", "rgb(102, 102, 102)", "rgba(119, 119, 119, 1)" , "rgba(102, 102, 102, 1)" };

        public static  bool IsRed(string color)
        {
            return Red.Contains(color);
        }

        public static  bool IsGrey(string color)
        {
            return Grey.Contains(color);
        }
    }
}
