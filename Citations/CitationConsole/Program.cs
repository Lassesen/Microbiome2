using CitationUtilities;
using System;

namespace CitationConsole
{
    class Program
    {
        static void Main(string[] args)
        {
          foreach(string arg in args)
            {
                int pmid = 0;
                if(int.TryParse(arg,out pmid))
                {
                    var test = new PubMed(pmid);
                    test.SaveToDatabase();
                }
                else
                {
                    var list = PubMed.SearchForArticles(arg);
                    foreach(var item in list)
                    {
                        var test = new PubMed(item);
                        test.SaveToDatabase();
                    }

                }
            }
        }
    }
}
