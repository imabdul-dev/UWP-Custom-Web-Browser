using System.Collections.Generic;
using System.Linq;
using App3.Core.Helpers;
using App3.Core.Models;
using Realms;

namespace App3.Core.Service
{
    public static class BookmarkDataService
    {
        private static IEnumerable<Bookmark> DefaultBookmarks()
        {
            return new List<Bookmark>
            {
                new Bookmark
                {
                    Id = 1,
                    Title = "Google",
                    Url = "http://www.google.com",
                    Favicon = FaviconHelper.GetFavicon("google.com")
                },
                new Bookmark
                {
                    Id = 2,
                    Title = "LinkedIn",
                    Url = "http://www.linkedin.com",
                    Favicon = FaviconHelper.GetFavicon("linkedin.com")
                },
                new Bookmark
                {
                    Id = 3,
                    Title = "YouTube",
                    Url = "http://www.youtube.com",
                    Favicon = FaviconHelper.GetFavicon("youtube.com")
                },
                new Bookmark
                {
                    Id = 4,
                    Title = "FaceBook",
                    Url = "http://www.facebook.com",
                    Favicon = FaviconHelper.GetFavicon("facebook.com")
                },
                new Bookmark
                {
                    Id = 5,
                    Title = "Twitter",
                    Url = "http://www.twitter.com",
                    Favicon = FaviconHelper.GetFavicon("twitter.com")
                },
                new Bookmark
                {
                    Id = 6,
                    Title = "Microsoft",
                    Url = "http://www.microsoft.com",
                    Favicon = FaviconHelper.GetFavicon("microsoft.com")
                },
                new Bookmark
                {
                    Id = 7,
                    Title = "Github",
                    Url = "http://www.github.com",
                    Favicon = FaviconHelper.GetFavicon("github.com")
                },
                new Bookmark
                {
                    Id = 999,
                    Title = "Add",
                    Url = "Add",
                    Favicon = "https://www.pngrepo.com/png/154811/170/thin-add-button.png"
                }
            };
        }

        public static IEnumerable<Bookmark> GetBookmarksData()
        {
            var realm = Realm.GetInstance();
            return realm.All<Bookmark>().OrderBy(x => x.Id);
        }

        public static void AddBookmark(string title, string url)
        {
            var authority = FaviconHelper.GetAuthority(url);
            var realm = Realm.GetInstance();
            using (var trans = realm.BeginWrite())
            {
                var bookmark = new Bookmark
                {
                    Id = GetBookmarksData().Count(),
                    Title = title,
                    Url = url,
                    Favicon = FaviconHelper.GetFavicon(authority)
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
    }
}