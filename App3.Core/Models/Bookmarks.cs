using Realms;

namespace App3.Core.Models
{
    public class Bookmark : RealmObject
    {
        public int Index { get; set; }
        public string Title { get; set; }
        public string Url { get; set; }
        public string Favicon { get; set; }
    }
}