using HtmlAgilityPack;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Data.SqlTypes;

using System.Net;
using System.Xml;

namespace CitationUtilities
{
    public class PubMed
    {
        public string ConnectionString = "Server=LAPTOP-BQ764BQT;Database=MicrobiomeV2;Trusted_Connection=True; Connection Timeout=1000";
        public PubMed(int pmid)
        {
            PmId = pmid;
            if (pmid < 1) return;
            CitationJson = GetPubMedCitation(pmid).Replace(pmid.ToString(),"article");
            
            PubMedAbstract = JsonConvert.DeserializeObject<Models.PubMed>(CitationJson, Models.Converter.Settings);
            var xml = GetPubMedXml(pmid);
            var dom = new XmlDocument();
            dom.LoadXml(xml);
            XmlNode abstractNode = dom.SelectSingleNode("//AbstractText");
            if(abstractNode != null)
            {
                Summary = abstractNode.InnerText;
            }

            Title = PubMedAbstract.Result.Article.Title;
            foreach (var item in  PubMedAbstract.Result.Article.Articleids)
            {
                if (item.Value.IndexOf("Article", StringComparison.OrdinalIgnoreCase) != 0)
                {
                    if (item.Idtype.Equals("doi", StringComparison.OrdinalIgnoreCase)  )
                    {
                        Doi = item.Value;
                    }
                    else if (item.Idtype.Equals("pii", StringComparison.OrdinalIgnoreCase) )
                    {
                        Pii = item.Value;
                    }
                    else if (item.Idtype.Equals("pmc", StringComparison.OrdinalIgnoreCase)) 
                    {
                        PmcId= item.Value;
                    }
                    else
                    {
                        Console.WriteLine(item.Idtype);
                    }
                }
            }
            if(!string.IsNullOrWhiteSpace(PmcUrl))
            {
                HtmlWeb web = new HtmlWeb();
                HtmlDocument doc=web.Load(PmcUrl);
                var ftextNode = doc.DocumentNode.SelectSingleNode("//div[@class='jig-ncbiinpagenav']");
                //<div class="jig-ncbiinpagenav"
                if(ftextNode != null)
                {
                    RawText = WebUtility.HtmlDecode( ftextNode.InnerText);
                }
            }
        }
        public Models.PubMed PubMedAbstract { get; }
        public string CitationJson { get; }
        public int PmId {get;}
        public string PmUrl { get { return PmId > 0 ? $"https://www.ncbi.nlm.nih.gov/pubmed/{PmId}" : ""; } }
        public string PmcId {get;}
        public string PmcUrl { get { return string.IsNullOrWhiteSpace(PmcId) ? "":$"https://www.ncbi.nlm.nih.gov/pmc/articles/{PmcId}/" ; } }
        public string Doi { get; }
        public string DoiUrl { get { return String.IsNullOrWhiteSpace(Doi) ? "": $"https://doi.org/{Doi}"; } }
        public string Pii { get; }
        public int CitationId { get; }
        public string RawText { get; set; }
        public string Summary { get; set; }
        public string Title { get; set; }

        private static string GetPubMedCitation(int pmid)
        {
            string url = $"https://eutils.ncbi.nlm.nih.gov/entrez/eutils/esummary.fcgi?db=pubmed&id={pmid}&retmode=json";
            try
            {
                using (System.Net.Http.HttpClient client = new System.Net.Http.HttpClient())
                {
                    return client.GetStringAsync(url).Result;//.Replace(id, "Article");string
                }
            }
            catch { return string.Empty; }
        }

        private static string GetPubMedXml(int pmid)
        {
            string url = $"http://eutils.ncbi.nlm.nih.gov/entrez/eutils/efetch.fcgi?db=pubmed&id={pmid}&retmode=xml";
            try
            {
                using (System.Net.Http.HttpClient client = new System.Net.Http.HttpClient())
                {
                    return client.GetStringAsync(url).Result;//.Replace(id, "Article");string
                }
            }
            catch { return string.Empty; }
        }
       
        public int SaveToDatabase()
        {
            if (String.IsNullOrWhiteSpace(Title))
            {
                throw new Exception("A title is required");
            }
            using (SqlConnection conn = new SqlConnection(ConnectionString))
            {
                conn.Open();
                using (SqlCommand cmd = new SqlCommand("uploadPubMed", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                     cmd.Parameters.AddWithValue("@Title", Title);
                    if(! string.IsNullOrWhiteSpace(Pii)) cmd.Parameters.AddWithValue("@Pii", Pii);
                    if (!string.IsNullOrWhiteSpace(Doi)) cmd.Parameters.AddWithValue("@Doi", Doi);
                    if (!string.IsNullOrWhiteSpace(PmcId)) cmd.Parameters.AddWithValue("@PmcId", PmcId);
                    if (!string.IsNullOrWhiteSpace(CitationJson)) cmd.Parameters.AddWithValue("@CitationJson", CitationJson);
                    if (!string.IsNullOrWhiteSpace(Summary)) cmd.Parameters.AddWithValue("@Summary", Summary);
                    if (PmId> 0) cmd.Parameters.AddWithValue("@PmId", PmId);
                    if (!string.IsNullOrWhiteSpace(RawText)) cmd.Parameters.AddWithValue("@RawText", RawText);

                    return int.Parse(cmd.ExecuteScalar().ToString());
                }
            }
            return 0;
        }
        public static List<int> SearchForArticles(string term,int maxresults=100)
        {
            List<int> result = new List<int>();
            string url = $"https://eutils.ncbi.nlm.nih.gov/entrez/eutils/esearch.fcgi?db=pubmed&term={term}&retmode=json&retmax={maxresults}";
            try
            {
                using (System.Net.Http.HttpClient client = new System.Net.Http.HttpClient())
                {
                    var searchResults = JsonConvert.DeserializeObject<Models.SearchResult>(client.GetStringAsync(url).Result, Models.Converter.Settings);
                    foreach (string item in searchResults.esearchresult.idlist)
                    {
                        int pmid = 0;
                        if(int.TryParse(item ,out pmid))
                        {
                            result.Add(pmid);
                        }
                    }
                }
            }
            catch (Exception exc)
            {
                Console.WriteLine(exc);
            }
            return result;
        }
    }
}
