using Game_Net_DTOLib;

namespace My_steam_server.Models
{
    public class Game : Good
    {

        public string? Description { get; set; } = string.Empty;
        public List<Screenshot> imageSource {  get; set; } = new();
        public string? HeaderImageSource { get; set; } = string.Empty;
        
        public User? User { get; set; }

        public string? DownloadSource { get; set; } = string.Empty;
        public Ratinng ratinng { get; set; }

        public DateTime ReleaseDate { get; set; }
        public List<PurchaseOption> PurchaseOptions { get; set; } = new();

        public override bool Equals(object? obj)
        {
            if (obj is Game other)
            {

                if (Name == other.Name) return true;
                if(imageSource == other.imageSource) return true;

            }
            return false;
        }
    }
}
