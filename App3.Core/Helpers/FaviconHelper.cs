using System;

namespace App3.Core.Helpers
{
    public static class FaviconHelper
    {
        public static string GetAuthority(string url)
        {
            var res = Uri.TryCreate(url, UriKind.Absolute, out var dom);
            return res ? dom.Authority : url;
        }

        public static string GetFavicon(string domain)
        {
            return $"https://api.faviconkit.com/{domain}/144";
        }
    }
}