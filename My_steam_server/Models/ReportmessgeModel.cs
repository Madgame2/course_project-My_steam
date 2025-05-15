using Game_Net_DTOLib;
using System.Text.Json.Serialization;

namespace My_steam_server.Models
{
    public class ReportMessageModel
    {
        public string UserID { get; set; }
        [JsonIgnore]
        public User User { get; set; }


        public string Message { get; set; } 

        public ReportToppic Topic { get; set; }

        public DateTime ReportDate { get; set; }
    }
}
