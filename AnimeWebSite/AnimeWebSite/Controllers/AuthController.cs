using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using AnimeWebApp;
using AnimeWebApplication.Models;
using AnimeWebSite.Hashing;
using AnimeWebSite.Models;
using AnimeWebSite.Options;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace AnimeWebSite.Controllers
{
    public class AuthController : Controller
    {
        private readonly IJWTAuthenticationManager jWTAuthenticationManager;
        private ApplicationDbContext _db;
        public AuthController(IJWTAuthenticationManager jWTAuthenticationManager, ApplicationDbContext context)
        {
            this.jWTAuthenticationManager = jWTAuthenticationManager;
            _db = context;
        }
        [HttpGet]
        public IActionResult Auth()
        {
            if (Request.Cookies["token"] != null)
            {
                return Redirect("/home/index");
            }
            else
            {
                return View();
            }
        }

        [HttpGet]
        public IActionResult LogIn()
        {
            if (Request.Cookies["token"] != null)
            {
                return Redirect("/home/index");
            }
            else
            {
                return View();
            }
        }
        
        [HttpPost]
         public IActionResult Registration([FromForm]Registration registration)
         {
             if (ModelState.IsValid && registration.Password == registration.ConfirmPassword)
             {
                 var user = _db.Users.FirstOrDefault(u => u.Email == registration.Email);

                 if (user == null)
                 {
                     var newUser = new User()
                     {
                         UserId = Guid.NewGuid(),
                         Email = registration.Email,
                         Nickname = registration.Nickname,
                         Password = Hasher.Encrypt(registration.Password)
                     };
                     var token = jWTAuthenticationManager.Authenticate(newUser);
                     Response.Cookies.Append("token", token);

                     var newProfile = new Profile()
                     {
                         Id = newUser.UserId
                     };
                     _db.Users.Add(newUser);
                     _db.Profiles.Add(newProfile);
                     _db.SaveChanges();

                     return Redirect("/home/index");
                 }
                 else
                 {
                     ModelState.AddModelError("", $"Такой пользователь уже существует");
                 }
             }
             else
             {
                 ModelState.AddModelError("", $"Не все поля заполнены.");
             }
             
             return Redirect("/home/index");
         }

        [HttpPost]
         public IActionResult Login([FromForm]Autithication autithication)
         {
             var currentUser = _db.Users.FirstOrDefault(u => u.Email == autithication.Email 
                                                             && u.Password == Hasher.Encrypt(autithication.Password));

             if (currentUser != null)
             {
                 if (currentUser.Password == Hasher.Encrypt(autithication.Password))
                 {
                     var token = jWTAuthenticationManager.Authenticate(currentUser);
                     Response.Cookies.Append("token", token);
                     return Redirect("/home/index");
                 }
                 else
                 {
                     ModelState.AddModelError("", $"Неверный пароль");
                 }
             }
             else
             {
                 return Redirect("/auth/auth");
             }
             
             return Redirect("/home/index");
         }

         public RedirectToActionResult Logout()
         {
             var token = Request.Cookies["token"];
             if (token == null)
                 return RedirectToAction("Login", "Auth");
             Response.Cookies.Delete("token");
             return RedirectToAction("Index", "Home");
         }
    }
}