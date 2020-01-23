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
        public static void AddorUpdateHistoryItem(History history)
        {
            var realm = Realm.GetInstance();
            var item = realm.Find<History>(history.Id);
            using (var trans = realm.BeginWrite())
            {
                if (item == null)
                {
                    var auth = FaviconHelper.GetAuthority(item.Url);
                    var fav = FaviconHelper.GetFavicon(auth);
                    item.Id = GetHistoryData().Count() + 1;
                    item.Favicon = fav;
                    item.Visted = DateTime.Now.ToString("D");

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