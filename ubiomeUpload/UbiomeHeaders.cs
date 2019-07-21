using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace UbiomeUpload
{
    public class UbiomeHeaders
    {
        [JsonProperty("download_time_utc")]
        public string DownloadTimeUtc { get; set; }

        [JsonProperty("sequencing_revision")]
        public string SequencingRevision { get; set; }

        [JsonProperty("site")]
        public string Site { get; set; }

        [JsonProperty("sampling_time")]
        public string SamplingTime { get; set; }

        [JsonProperty("notes")]
        public string Notes { get; set; }

        [JsonProperty("ubiome_bacteriacounts")]
        public UbiomeCount[] UbiomeBacteriacounts { get; set; }
    }
}
