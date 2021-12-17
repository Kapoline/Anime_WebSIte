using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using AnimeWebApp;
using AnimeWebSite.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace AnimeWebSite.Controllers
{
    public class AnimeItemController : Controller
    {
        private ApplicationDbContext _db;
        private readonly IJWTAuthenticationManager jWTAuthenticationManager;
        private Tools.Tools _tools;
        IWebHostEnvironment _appEnvironment;

        public AnimeItemController(ApplicationDbContext context, IJWTAuthenticationManager jWTAuthenticationManager, IWebHostEnvironment appEnvironment)
        {
            _db = context;
            this.jWTAuthenticationManager = jWTAuthenticationManager;
            _tools = new Tools.Tools(context, jWTAuthenticationManager);
            _appEnvironment = appEnvironment;
        }
        public IActionResult AddAnimeItem()
        {
            return View();
        }

        public JsonResult OnPostAddItem(
            [FromForm] string name,
            [FromForm] string description,
            [FromForm] string year,
            [FromForm] string ganre,
            [FromForm] string director,
            [FromForm] string countOfSeries
            )
        {
            var item = new AnimeItem()
            {
                ItemId = Guid.NewGuid(),
                Name = name,
                Description = description,
                Year = Convert.ToInt32(year),
                Genre = ganre,
                Director = director,
                SeriesCount = Convert.ToInt32(countOfSeries)
            };

            _db.AnimeItems.Add(item);
            _db.SaveChanges();

            return new JsonResult(item.ItemId.ToString());
        }
        
        public async Task<JsonResult> OnPostUploadPhoto(IFormFile uploadedFile, string itemId)
        {
            var animeItem = _db.AnimeItems.FirstOrDefault(item => item.ItemId == Guid.Parse(itemId));
            var photoPath = "";
            if (uploadedFile != null)
            {
                string path = "/AnimePosters/" + animeItem.Name + uploadedFile.FileName;
                using (var fileStream = new FileStream(_appEnvironment.WebRootPath + path, FileMode.Create))
                {
                    await uploadedFile.CopyToAsync(fileStream);
                }

                animeItem.PosterPath = path;
                photoPath = path;
            }
            
            _db.AnimeItems.Update(animeItem);
            _db.SaveChanges();
            return new JsonResult(photoPath);
        }
    }
}