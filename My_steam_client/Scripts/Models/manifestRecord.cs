using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace My_steam_client.Scripts.Models
{
    public class ManifestRecord
    {
        public long RecordId {  get; set; }
        public string UserId {  get; set; }
        public long GameId {  get; set; }
        public string GameName { get; set; }
        public string DownloadSource {  get; set; }
        public LibElemStatuses libElemStaus { get; set; } = LibElemStatuses.Not_instaled;

        public string LibIconSource { get; set; }
        public string HeaderImageSource { get; set; }
        public string? ExecuteFileSource {  get; set; }

        public long SpaceRequered {  get; set; }

        public DateTime PurhcaseDate { get; set; }
        public DateTime lastPlayed { get; set; }
        public TimeSpan playedTime { get; set; }
    }
}
