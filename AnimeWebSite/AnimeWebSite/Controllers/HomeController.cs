using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using AnimeWebApp;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using AnimeWebSite.Models;
using Microsoft.AspNetCore.Http;

namespace AnimeWebSite.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private ApplicationDbContext _db;
        private readonly IJWTAuthenticationManager jWTAuthenticationManager;
        private Tools.Tools _tools;

        public HomeController(ILogger<HomeController> logger, ApplicationDbContext context, IJWTAuthenticationManager jWTAuthenticationManager)
        {
            _logger = logger;
            _db = context;
            this.jWTAuthenticationManager = jWTAuthenticationManager;
            _tools = new Tools.Tools(context, jWTAuthenticationManager);
        }

        public IActionResult Index()
        {
            if (Request.Cookies["token"] != null)
            {
                var user = _tools.FindUserByToken(Request);
                return View(user);
            }
            else
            {
                return View();
            }
        }

        public IActionResult Privacy()
        {
            return View();
        }

        public IActionResult HomePage()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel {RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier});
        }

        public JsonResult OnGetAnimeItems()
        {
            var items = _db.AnimeItems.ToList();
            var animteItems = new Dictionary<string, List<AnimeItem>>();
            animteItems.Add("items", items);
            
            var result = JsonSerializer.Serialize<Dictionary<string, List<AnimeItem>>>(animteItems);

            return new JsonResult(result);
        }

        public JsonResult OnPostDeleteButton(string itemId)
        {
            var item = _db.AnimeItems.FirstOrDefault(i => i.ItemId == Guid.Parse(itemId));
            _db.AnimeItems.Remove(item);
            _db.SaveChanges();

            return new JsonResult("ok");
        }
    }
}