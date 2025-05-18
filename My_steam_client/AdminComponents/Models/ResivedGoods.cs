using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace My_steam_client.AdminComponents.Models
{
    public class ResivedGoods : INotifyPropertyChanged
    {
        public long Id { get; set; }
        public long GoodId {  get; set; }
        public long PurhcaseOptinId {  get; set; }

        [Browsable(false)]
        public bool IsNew { get; set; } = false;

        public event PropertyChangedEventHandler? PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string? prop = null)
    => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));
    }
}
