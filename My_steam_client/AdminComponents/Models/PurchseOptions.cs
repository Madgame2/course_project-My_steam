using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace My_steam_client.AdminComponents.Models
{
    public class PurchseOptions : INotifyPropertyChanged
    {
        public long PurhcaseId {  get; set; }
        public float Price {  get; set; }
        public string PurhcaseName {  get; set; } = string.Empty;
        public string imageLinnk { get; set; } = string.Empty;
        public long GameID { get; set; }


        [Browsable(false)]
        public bool IsNew { get; set; } = false;

        public event PropertyChangedEventHandler? PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string? prop = null)
    => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));
    }
}
