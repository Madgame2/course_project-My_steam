using Game_Net_DTOLib;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace My_steam_client.AdminComponents.Models
{
    public class Games : INotifyPropertyChanged
    {
        public long GameId {  get; set; }
        public string GameName { get; set; }=string.Empty;
        public string Descritption {  get; set; }=string.Empty;
        public string HeaderImageSource { get; set; } = string.Empty;
        public string UserId { get; set; } = string.Empty;
        public string DownloadedSource { get; set; } = string.Empty;
        public Ratinng Rating { get; set; }
        public DateTime ReliseDate { get; set; }
        public float Price {  get; set; }

        [Browsable(false)]
        public bool IsNew { get; set; } = false;

        public event PropertyChangedEventHandler? PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string? prop = null)
    => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));
    }
}
