using System;
using System.Collections.Generic;
using System.Linq;
using App3.Core.Models;
using Realms;

namespace App3.Core.Service
{
    public static class BookmarkDataService
    {
        private static IEnumerable<Bookmark> DefaultBookmarks()
        {
            return new List<Bookmark>()
            {
                new Bookmark
                {
                    Index = 0,
                    Title = "Google",
                    Url = "http://www.google.com",
                    Favicon = GetFavicon("google.com")
                },
                new Bookmark
                {
                    Index = 1,
                    Title = "LinkedIn",
                    Url = "http://www.linkedin.com",
                    Favicon = GetFavicon("linkedin.com")
                },
                new Bookmark
                {
                    Index = 2,
                    Title = "YouTube",
                    Url = "http://www.youtube.com",
                    Favicon = GetFavicon("youtube.com")
                },
                new Bookmark
                {
                    Index = 3,
                    Title = "FaceBook",
                    Url = "http://www.facebook.com",
                    Favicon = GetFavicon("facebook.com")
                },
                new Bookmark
                {
                    Index = 4,
                    Title = "Twitter",
                    Url = "http://www.twitter.com",
                    Favicon = GetFavicon("twitter.com")
                },
                new Bookmark
                {
                    Index = 5,
                    Title = "Microsoft",
                    Url = "http://www.microsoft.com",
                    Favicon = GetFavicon("microsoft.com")
                },
                new Bookmark
                {
                    Index = 6,
                    Title = "Github",
                    Url = "http://www.github.com",
                    Favicon = GetFavicon("github.com")
                },
                new Bookmark
                {
                    Index = 999,
                    Title = "Add",
                    Url = "Add",
                    Favicon = "https://www.pngrepo.com/png/154811/170/thin-add-button.png"
                }
            };
        }

        public static string GetFavicon(string domain)
        {
            return $"https://api.faviconkit.com/{domain}/144";
        }

        public static IEnumerable<Bookmark> GetBookmarksData()
        {
            var realm = Realm.GetInstance();
            return realm.All<Bookmark>().OrderBy(x => x.Index);
        }

        public static void AddBookmark(string title, string url)
        {
            var authority = GetAuthority(url);
            var realm = Realm.GetInstance();
            using (var trans = realm.BeginWrite())
            {
                var bookmark = new Bookmark
                {
                    Title = title,
                    Url = url,
                    Favicon = GetFavicon(authority)
                };

                realm.Add(bookmark);
                trans.Commit();
            }
        }

        public static void AddBookmarks()
        {
            var realm = Realm.GetInstance();
            var bookmarks = DefaultBookmarks();
            using (var trans = realm.BeginWrite())
            {
                realm.RemoveAll<Bookmark>();
                foreach (var bookmark in bookmarks)
                {
                    realm.Add(bookmark);
                }
                trans.Commit();
            }
        }

        private static string GetAuthority(string url)
        {
            var res = Uri.TryCreate(url, UriKind.Absolute, out var dom);
            return res ? dom.Authority : url;
        }
    }
}