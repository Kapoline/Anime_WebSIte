using AnimeWebSite.Models;

namespace AnimeWebApp
{
    public interface IJWTAuthenticationManager
    {
        public string Authenticate(User model);
    }
}