using System.Text;
using Microsoft.IdentityModel.Tokens;

namespace AnimeWebSite.Options
{
    public class AuthOptions
    {
        public string Issuer { get; set; }
        public string Audience { get; set; }
        public static string Secret { get; set; }
        public int LifeTime { get; set; }
        public SymmetricSecurityKey GetSymmetricSecurityKey()
        {
            return new SymmetricSecurityKey(Encoding.ASCII.GetBytes(Secret));
        }
    }
}