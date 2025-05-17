using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Game_Net_DTOLib
{
    public class ProjectUploadDto
    {
        public string UserId {  get; set; }
        public string ProjectName { get; set; }
        public string Description { get; set; }
        public float Price { get; set; }

        public Stream HeaderImage { get; set; }
        public Stream ZIPFile { get; set; }
        public Stream LibHeader { get; set; }
        public Stream LibIcon { get; set; }

        public List<Stream> Screenshots { get; set; }
    }
}
