using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace My_steam_client.Models
{
    public class Project
    {
        public long ProjectId {  get; set; }
        public string ProjectName {  get; set; }
        public string ProjectDescription {  get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
