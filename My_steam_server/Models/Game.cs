using Game_Net_DTOLib;

namespace My_steam_server.Models
{
    public class Game : Good
    {

        public string Description { get; set; }
        public string[] imageSource {  get; set; }
        public string HeaderImageSource { get; set; }

        public string MdFileSorce {  get; set; }
        public Ratinng ratinng { get; set; }

        public DateTime ReleaseDate { get; set; }
        public Game_Net_DTOLib.PurchaseOption[] PurchaseOption { get; set; }

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
