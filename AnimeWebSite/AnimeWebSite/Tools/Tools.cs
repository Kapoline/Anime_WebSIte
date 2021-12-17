using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using AnimeWebApp;
using AnimeWebSite.Models;
using Microsoft.AspNetCore.Http;

namespace AnimeWebSite.Tools
{
    public class Tools
    {
        private readonly IJWTAuthenticationManager jWTAuthenticationManager;
        private ApplicationDbContext _db;
        
        public Tools(ApplicationDbContext context, IJWTAuthenticationManager jWTAuthenticationManager)
        {
            _db = context;
            this.jWTAuthenticationManager = jWTAuthenticationManager;
        }
        public User FindUserByToken(HttpRequest request)
        {
            var stream = request.Cookies["token"];
            var handler = new JwtSecurityTokenHandler();
            var jsonToken = handler.ReadToken(stream);
            var tokenS = jsonToken as JwtSecurityToken;
            var id = tokenS.Claims.First(claim => claim.Type == "nameid").Value;
            var user = _db.Users.FirstOrDefault(u => u.UserId.ToString() == id);
            return user;
        }
    }
}