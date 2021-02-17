using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace DataFinder.Models
{
    public class UrlSearch
    {
        public int Id { get; set; }
        public User User { get; set; }
        public int UserId { get; set; }
        public DateTime SearchDate { get; set; }
        public string SearchUrl { get; set; }
        public string SearchedTerm { get; set; }
        public int CountFoundTerm { get; set; }
    }
}