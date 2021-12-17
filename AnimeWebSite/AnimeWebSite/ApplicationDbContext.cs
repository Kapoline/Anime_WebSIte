using AnimeWebApplication.Models;
using AnimeWebSite.Models;
using Microsoft.EntityFrameworkCore;

namespace AnimeWebSite
{
    public class ApplicationDbContext : DbContext
    {
        public DbSet<User> Users { get; set; }
        public DbSet<Profile> Profiles { get; set; }
        public DbSet<AnimeItem> AnimeItems { get; set; }

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
            
        }
    }
}