using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using AnimeWebApplication.Models;
using LinqToDB.Mapping;

namespace AnimeWebSite.Models
{
    public class User
    {
        public Guid UserId { get; set; }
        public string Email { get; set; }
        public string Nickname { get; set; }
        public string Password { get; set; }
        
        public Profile Profile { get; set; }
        
    }
}