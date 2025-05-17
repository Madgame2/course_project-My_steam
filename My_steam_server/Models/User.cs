using Game_Net_DTOLib;
using Microsoft.AspNetCore.Identity;

namespace My_steam_server.Models
{
    public class User : IdentityUser
    {
        public string Nickname { get; set; }
        public UserRole Role { get; set; }
        public DateTime RigisterDate { get; set; }

        public List<CartItem> CartItems { get; set; } = new();
        public List<UserLibraryEntry> Library { get; set; } = new();
        public List<Game> Games { get; set; } = new();
    }
}
