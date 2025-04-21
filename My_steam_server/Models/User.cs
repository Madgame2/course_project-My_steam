using Microsoft.AspNetCore.Identity;

namespace My_steam_server.Models
{
    public class User : IdentityUser
    {
        public string Nickname { get; set; }
    }
}
