using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Game_Net_DTOLib
{
    public class ProjectUploadDto
    {
        public string ProjectName { get; set; }
        public string Description { get; set; }
        public float Price { get; set; }

        public byte[] HeaderImage { get; set; }
        public Stream ZIPFile { get; set; }
        public byte[] LibHeader { get; set; }
        public byte[] LibIcon { get; set; }

        public List<byte[]> Screenshots { get; set; }
    }
}
