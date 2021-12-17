using System.IO;
using System.Linq;
using System.Threading.Tasks;
using AnimeWebApp;
using AnimeWebApplication.Models;
using AnimeWebSite.Models;
using LinqToDB;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace AnimeWebSite.Controllers
{
    public class AccountController : Controller
    {
        private ApplicationDbContext _db;
        private readonly IJWTAuthenticationManager jWTAuthenticationManager;
        private Tools.Tools _tools;
        IWebHostEnvironment _appEnvironment;

        public AccountController( ApplicationDbContext context, IJWTAuthenticationManager jWTAuthenticationManager, IWebHostEnvironment appEnvironment)
        {
            _db = context;
            this.jWTAuthenticationManager = jWTAuthenticationManager;
            _tools = new Tools.Tools(context, jWTAuthenticationManager);
            _appEnvironment = appEnvironment;
        }
        
        public IActionResult EditProfile()
        {
            return View();
        }

        public IActionResult UserProfile()
        {
            if (Request.Cookies["token"] == null)
            {
                return Redirect("/auth/auth");
            }
            else
            {
                var currentUser = _tools.FindUserByToken(Request);
                var currentProfile = _db.Profiles.FirstOrDefault(profile => profile.Id == currentUser.UserId);
                var profileModel = new ProfileModel()
                {
                    Email = currentUser.Email,
                    Nickname = currentUser.Nickname,
                    Birthday = currentProfile.Birthday,
                    City = currentProfile.City,
                    Description = currentProfile.Description,
                    PhotoPath = currentProfile.PhotoPath
                };
                return View(profileModel);
            }
        }

        public JsonResult OnPostEditProfile(
            [FromForm]string birthday, 
            [FromForm]string sex, 
            [FromForm]string city, 
            [FromForm]string description)
        {
            var currentUser = _tools.FindUserByToken(Request);
            var currentProfile = _db.Profiles.FirstOrDefault(profile => profile.Id == currentUser.UserId);
            currentProfile.Birthday = birthday;
            currentProfile.Sex = sex;
            currentProfile.City = city;
            currentProfile.Description = description;

            _db.Profiles.Update(currentProfile);
            _db.SaveChanges();

            return new JsonResult("ok");
        }
        
        public async Task<JsonResult> OnPostUploadPhoto(IFormFile uploadedFile)
        {
            var currentUser = _tools.FindUserByToken(Request);
            var currentProfile = _db.Profiles.FirstOrDefault(profile => profile.Id == currentUser.UserId);
            var photoPath = "";
            if (uploadedFile != null)
            {
                string path = "/UserAvatars/"+ currentUser.Nickname + uploadedFile.FileName;
                using (var fileStream = new FileStream(_appEnvironment.WebRootPath + path, FileMode.Create))
                {
                    await uploadedFile.CopyToAsync(fileStream);
                }
                photoPath = path;
                currentProfile.PhotoPath = photoPath;
                _db.Profiles.Update(currentProfile);
                _db.SaveChanges();
            }
            return new JsonResult(photoPath);
        }
    }
}