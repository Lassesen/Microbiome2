using Newtonsoft.Json;


namespace MicrobiomeLibrary.Uploaders
{
    public class UbiomeCount
    {
        [JsonProperty("taxon")]
        public int Taxon { get; set; }

        [JsonProperty("parent")]
        public int Parent { get; set; }

        [JsonProperty("count")]
        public int Count { get; set; }

        [JsonProperty("count_norm")]
        public int CountNorm { get; set; }

        [JsonProperty("tax_name")]
        public string TaxName { get; set; }

        [JsonProperty("tax_rank")]
        public string TaxRank { get; set; }
    }
}
