using System;
using System.Text.RegularExpressions;

namespace App3.Helpers
{
    public static class WebHelper
    {
        public static string Url { get; set; } = "https://developer.microsoft.com/en-us/windows/apps";
        public static Uri ValidURL(string Url)
        {
            var Pattern = @"^(?:http(s)?:\/\/)?[\w.-]+(?:\.[\w\.-]+)+[\w\-\._~:/?#[\]@!\$&'\(\)\*\+,;=.]+$";
            var Rgx = new Regex(Pattern, RegexOptions.Compiled | RegexOptions.IgnoreCase);
            var result = Rgx.IsMatch(Url);

            if (!result)
            {
                return new Uri($"https://www.google.com/search?q={Url}");
            }

            if (Url.Length > 7)
            {
                var sub = Url.Substring(0, 7);
                if (!sub.Contains("https://") && !sub.Contains("http://"))
                {
                    Url = $"http://{Url}";
                }
            }
            else
            {
                Url = $"http://{Url}";
            }

            return new Uri(Url);
        }
    }
}
