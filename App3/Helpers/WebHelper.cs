using System;
using System.Text.RegularExpressions;

namespace UWPBrowser.Helpers
{
    public static class WebHelper
    {
        public static string Url { get; set; } = "https://www.google.com";

        public static Uri ValidURL(string url)
        {
            const string Pattern = @"^(?:http(s)?:\/\/)?[\w.-]+(?:\.[\w\.-]+)+[\w\-\._~:/?#[\]@!\$&'\(\)\*\+,;=.]+$";
            var Rgx = new Regex(Pattern, RegexOptions.Compiled | RegexOptions.IgnoreCase);
            var result = Rgx.IsMatch(url);

            if (!result)
            {
                return new Uri($"https://www.google.com/search?q={url}");
            }

            if (url.Length > 7)
            {
                var sub = url.Substring(0, 7);
                if (!sub.Contains("https://") && !sub.Contains("http://"))
                {
                    url = $"http://{url}";
                }
            }
            else
            {
                url = $"http://{url}";
            }

            return new Uri(url);
        }
    }
}
