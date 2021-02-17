using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using DataFinder.Models;
using DataFinder.Filter;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Http;
using System.Net;
using System.IO;
using System.Text.RegularExpressions;

namespace DataFinder.Controllers
{
    [UserFilter]
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly DataContext _context;

        public HomeController(DataContext context, ILogger<HomeController> logger)
        {
            _context = context;
            _logger = logger;
        }
        public IActionResult Index()
        {
            return View();
        }
        public IActionResult Privacy()
        {
            return View();
        }
        public IActionResult DataFind()
        {
            return View();
        }
        public IActionResult SearchResult()
        {
            return View();
        }
        public IActionResult Report()
        {
            List<UrlSearch> search = (from UrlSearch in _context.UrlSearch.Take(100) where UrlSearch.UserId==(int) HttpContext.Session.GetInt32("userId")
                                      select UrlSearch).ToList();
            return View(search);
        }
        public async Task<IActionResult> Search(string url, string searchedtext)
        {
            try
            {
                WebRequest request = HttpWebRequest.Create(url);
                WebResponse response = request.GetResponse();
                StreamReader reader = new StreamReader(response.GetResponseStream());
                string sourceText = reader.ReadToEnd();
                int counter = 0;
                int location = sourceText.IndexOf(searchedtext);
                while (location != -1)
                {
                    location = sourceText.IndexOf(searchedtext, location + 1);
                    counter++;
                }
                var model = new UrlSearch();
                model.UserId = (int)HttpContext.Session.GetInt32("userId");
                model.SearchDate = DateTime.Now;
                model.SearchUrl = url.Trim().ToString();
                model.SearchedTerm = searchedtext.ToString();
                model.CountFoundTerm = (int)counter;
                HttpContext.Session.SetString("result", "The Url  :" + url +  "\nThe Searched Term  :" + searchedtext + "\nCount   :" + counter);
                await _context.AddAsync(model);
                await _context.SaveChangesAsync();
            }

            catch 
            {
                HttpContext.Session.SetString("result", "The Url is not valid");
            }
            
            return Redirect("/Home/SearchResult");
        }
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
