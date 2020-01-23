using System;
using Realms;

namespace App3.Core.Models
{
    public class History : RealmObject
    {
        [PrimaryKey]
        public int Id { get; set; } 
        public string Title { get; set; }
        public string Url { get; set; }
        public string Favicon { get; set; }
        public string Visted { get; set; }
    }
}