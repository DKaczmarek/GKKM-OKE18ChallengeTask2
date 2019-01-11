using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VDS.RDF;
using VDS.RDF.Parsing;
using VDS.RDF.Query;
using System.IO;

namespace OKE2018_Challenge___Task_2
{
    class SPARQL
    {
        JSON_entity js = new JSON_entity();
        public SPARQL(JSON_entity js_e) {
            this.js = js_e;
        }

        public Phrase ask(Context context)
        {
            Phrase phrase = null;
            SparqlRemoteEndpoint endpoint = new SparqlRemoteEndpoint(new Uri("http://dbpedia.org/sparql"), "http://dbpedia.org");
            SparqlResultSet results = endpoint.QueryWithResultSet(@"SELECT ?x WHERE { ?x a dbo:" + js.label + @". ?x rdfs:label ?name . FILTER regex(str(?name), ""^" + js.name + @"$"")}LIMIT 1");
            foreach (SparqlResult result in results)
            {
                string res = result.ToString();
                res = res.Replace("?x = ", string.Empty);
                phrase = new Phrase(context, js.name, js.begin_index, js.end_index, res);
            }
            if (results.LongCount() == 0) {
                js.name = js.name.Replace(" ", "_");
                phrase = new Phrase(context, js.name, js.begin_index, js.end_index, "http://aksw.org/notInWiki/" + js.name);
        }
            return phrase;
        }
    }
}
