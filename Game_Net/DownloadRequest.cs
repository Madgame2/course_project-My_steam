using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Game_Net
{
    public class DownloadRequest
    {
        public long ManifestRecordId {  get; set; }
        public string URL {  get; set; }
        public string savePath { get; set; }

        public DownloadRequest(long RecoredId,string URL,string savePath)
        {
            ManifestRecordId = RecoredId;
            this.URL = URL;
            this.savePath = savePath;
        }
    }
}
