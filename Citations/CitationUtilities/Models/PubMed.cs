using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json;
using System.Globalization;

namespace CitationUtilities.Models
{
    public partial class PubMed
    {
        [JsonProperty("header")]
        public Header Header { get; set; }

        [JsonProperty("result")]
        public Result Result { get; set; }
    }

    public partial class Header
    {
        [JsonProperty("type")]
        public string Type { get; set; }

        [JsonProperty("version")]
        public string Version { get; set; }
    }

    public partial class Result
    {
        [JsonProperty("Article")]
        public Article Article { get; set; }

        [JsonProperty("uids")]
        public string[] Uids { get; set; }
    }

    public partial class Article
    {
        [JsonProperty("uid")]
        public string Uid { get; set; }

        [JsonProperty("pubdate")]
        public string Pubdate { get; set; }

        [JsonProperty("epubdate")]
        public string Epubdate { get; set; }

        [JsonProperty("source")]
        public string Source { get; set; }

        [JsonProperty("authors")]
        public Author[] Authors { get; set; }

        [JsonProperty("lastauthor")]
        public string Lastauthor { get; set; }

        [JsonProperty("title")]
        public string Title { get; set; }

        [JsonProperty("sorttitle")]
        public string Sorttitle { get; set; }

        [JsonProperty("volume")]
        public string Volume { get; set; }

        [JsonProperty("issue")]
        public string Issue { get; set; }

        [JsonProperty("pages")]
        public string Pages { get; set; }

        [JsonProperty("lang")]
        public string[] Lang { get; set; }

        [JsonProperty("nlmuniqueid")]
        public string Nlmuniqueid { get; set; }

        [JsonProperty("issn")]
        public string Issn { get; set; }

        [JsonProperty("essn")]
        public string Essn { get; set; }

        [JsonProperty("pubtype")]
        public string[] Pubtype { get; set; }

        [JsonProperty("recordstatus")]
        public string Recordstatus { get; set; }

        [JsonProperty("pubstatus")]
        public string Pubstatus { get; set; }

        [JsonProperty("articleids")]
        public Articleid[] Articleids { get; set; }

        [JsonProperty("history")]
        public History[] History { get; set; }

        [JsonProperty("references")]
        public object[] References { get; set; }

        [JsonProperty("attributes")]
        public string[] Attributes { get; set; }

        [JsonProperty("pmcrefcount")]
        public long? Pmcrefcount { get; set; }

        [JsonProperty("fulljournalname")]
        public string Fulljournalname { get; set; }

        [JsonProperty("elocationid")]
        public string Elocationid { get; set; }

        [JsonProperty("doctype")]
        public string Doctype { get; set; }

        [JsonProperty("booktitle")]
        public string Booktitle { get; set; }

        [JsonProperty("medium")]
        public string Medium { get; set; }

        [JsonProperty("edition")]
        public string Edition { get; set; }

        [JsonProperty("publisherlocation")]
        public string Publisherlocation { get; set; }

        [JsonProperty("publishername")]
        public string Publishername { get; set; }

        [JsonProperty("srcdate")]
        public string Srcdate { get; set; }

        [JsonProperty("reportnumber")]
        public string Reportnumber { get; set; }

        [JsonProperty("availablefromurl")]
        public string Availablefromurl { get; set; }

        [JsonProperty("locationlabel")]
        public string Locationlabel { get; set; }

        [JsonProperty("doccontriblist")]
        public object[] Doccontriblist { get; set; }

        [JsonProperty("docdate")]
        public string Docdate { get; set; }

        [JsonProperty("bookname")]
        public string Bookname { get; set; }

        [JsonProperty("chapter")]
        public string Chapter { get; set; }

        [JsonProperty("sortpubdate")]
        public string Sortpubdate { get; set; }

        [JsonProperty("sortfirstauthor")]
        public string Sortfirstauthor { get; set; }

        [JsonProperty("vernaculartitle")]
        public string Vernaculartitle { get; set; }
    }

    public partial class Articleid
    {
        [JsonProperty("idtype")]
        public string Idtype { get; set; }

        [JsonProperty("idtypen")]
        public long? Idtypen { get; set; }

        [JsonProperty("value")]
        public string Value { get; set; }
    }

    public partial class Author
    {
        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("authtype")]
        public string Authtype { get; set; }

        [JsonProperty("clusterid")]
        public string Clusterid { get; set; }
    }

    public partial class History
    {
        [JsonProperty("pubstatus")]
        public string Pubstatus { get; set; }

        [JsonProperty("date")]
        public string Date { get; set; }
    }

    internal class Converter
    {
        public static readonly JsonSerializerSettings Settings = new JsonSerializerSettings
        {
            MetadataPropertyHandling = MetadataPropertyHandling.Ignore,
            DateParseHandling = DateParseHandling.None,
            Converters = {
                new IsoDateTimeConverter()
                {
                    DateTimeStyles = DateTimeStyles.AssumeUniversal,
                },
            },
        };
    }
}
