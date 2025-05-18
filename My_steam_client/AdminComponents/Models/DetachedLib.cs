using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace My_steam_client.AdminComponents.Models
{
    internal class DetachedLib : INotifyPropertyChanged
    {
        public string UserId {  get; set; } = string.Empty;
        public long GameId { get; set; }
        public DateTime PurchaseDate { get; set; }


        [Browsable(false)]
        public bool IsNew { get; set; } = false;

        public event PropertyChangedEventHandler? PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string? prop = null)
    => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));
    }
}
