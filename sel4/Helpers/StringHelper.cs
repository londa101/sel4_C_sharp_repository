using System;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;

namespace sel4.Helpers
{
    class StringHelper
    {
        private static readonly Random RandomGenerator = new Random();
        public static string GetRandomString()
        {
            var path = Path.GetRandomFileName();
            path = path.Replace(".", string.Empty);  // Delete dot beetween name and extension
            return path;
        }

        public static string GetRandomString(int length)
        {
            var result = new StringBuilder("");
            for (var i = 0; i < length; i++)
            {
                char c;
                while (!Regex.IsMatch((c = Convert.ToChar(RandomGenerator.Next(48, 128))).ToString(), "[a-zA-Z]"))
                { }
                result.Append(c);
            }

            return result.ToString();
        }
        public static string GetRandomNumberString(int length)
        {
            var result = new StringBuilder("");
            for (var i = 0; i < length; i++)
            {
                char c;
                while (!Regex.IsMatch((c = Convert.ToChar(RandomGenerator.Next(48, 128))).ToString(), "[0-9]"))
                { }
                result.Append(c);
            }

            return result.ToString();
        }
    }
}
