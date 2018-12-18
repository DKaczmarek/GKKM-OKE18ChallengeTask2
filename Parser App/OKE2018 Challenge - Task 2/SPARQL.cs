using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VDS.RDF;
using VDS.RDF.Parsing;
using VDS.RDF.Query;

namespace OKE2018_Challenge___Task_2
{
    class SPARQL
    {
        string word, label;
        public SPARQL(string wo, string la) {
            this.word = wo;
            this.label = la;
        }

        public void ask()
        {
            SparqlRemoteEndpoint endpoint = new SparqlRemoteEndpoint(new Uri("http://dbpedia.org/sparql"), "http://dbpedia.org");
            SparqlResultSet results = endpoint.QueryWithResultSet(@"SELECT ?x WHERE { ?x a dbo:" + label + @". ?x rdfs:label ?name . FILTER regex(str(?name), ""^" + word + @"$"")}LIMIT 1");
            foreach (SparqlResult result in results)
            {
                string res=result.ToString();
                res = res.Replace("?x = ", string.Empty);
            }
        }
    }
}
