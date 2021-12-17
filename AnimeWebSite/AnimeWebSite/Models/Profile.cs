using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using AnimeWebSite.Models;

namespace AnimeWebApplication.Models
{
    public class Profile
    {
        [ForeignKey("User")]
        public Guid Id { get; set; }
        public string Birthday { get; set; }
        public string Sex { get; set; }
        public string City { get; set; }
        public string Description { get; set; }
        public string PhotoPath { get; set; }
        
        public User User { get; set; }
    }
}