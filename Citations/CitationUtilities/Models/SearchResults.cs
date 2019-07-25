using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace CitationUtilities.Models
{
    public class SearchResult
    {
        [JsonProperty("Header")]
        public SHeader header { get; set; }
        public Esearchresult esearchresult { get; set; }
    }

    public class SHeader
    {
        public static Header FromJson(string json) => JsonConvert.DeserializeObject<Header>(json, Models.Converter.Settings);

        public string type { get; set; }

        public string version
        {
            get; set;
        }
    }

    public class Translationset
    {
        public string from { get; set; }
        public string to { get; set; }
    }

    public class Esearchresult
    {
        public string count { get; set; }
        public string retmax { get; set; }
        public string retstart { get; set; }
        public List<string> idlist { get; set; }
        public List<Translationset> translationset { get; set; }
        public List<object> translationstack { get; set; }
        public string querytranslation { get; set; }
    }
}
