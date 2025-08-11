using System;

namespace DataFinder.Wpf.Models
{
    public class UrlSearch
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public DateTime SearchDate { get; set; }
        public string SearchUrl { get; set; }
        public string SearchedTerm { get; set; }
        public int CountFoundTerm { get; set; }
    }
}