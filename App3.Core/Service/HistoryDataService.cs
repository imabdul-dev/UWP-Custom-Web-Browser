using System;
using System.Collections.Generic;
using System.Linq;
using App3.Core.Helpers;
using App3.Core.Models;
using Realms;

namespace App3.Core.Service
{
    public static class HistoryDataService
    {
        public static void AddorUpdateHistoryItem(string title, string url)
        {
            var realm = Realm.GetInstance();

            var item = realm.All<History>().FirstOrDefault(x => x.Title == title && x.Url == url);
            using (var trans = realm.BeginWrite())
            {
                if (item == null)
                {
                    var auth = FaviconHelper.GetAuthority(url);
                    var fav = FaviconHelper.GetFavicon(auth);
                    item = new History
                    {
                        Id = GetHistoryData().Count() + 1,
                        Title = title,
                        Favicon = fav,
                        Visted = "Visited: " + DateTime.Now.ToString("D")
                    };

                    realm.Add(item);
                }
                else
                {
                    realm.Add(item, true);
                }

                trans.Commit();
            }
        }

        public static IEnumerable<History> GetHistoryData()
        {
            var realm = Realm.GetInstance();
            return realm.All<History>().OrderBy(x => x.Id);
        }
    }
}