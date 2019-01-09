using System;
using System.Text;

namespace OKE2018_Challenge___Task_2
{
    public abstract class Sentence
    {
        /// <summary> Example: <http://www.ontologydesignpatterns.org/data/oke-challenge-2017/task-1/sentence-28#char=303,320> </summary>
        public String uri;
        /// <summary> Example: nif:RFC5147String , nif:String , nif:Phrase ; </summary>
        public String a;
        /// <summary> Example: "303"^^xsd:nonNegativeInteger ; </summary>
        public int beginIndex;
        /// <summary> Example: "320"^^xsd:nonNegativeInteger ; </summary>
        public int endIndex;
    }

    /// <summary> Context is input file </summary>
    public class Context : Sentence
    {
        public String oryginalInput;
        /// <summary> Example: "JFK is a 1991 American historical legal-conspiracy thriller film" </summary>
        public String isString;
        /// <summary> Create Context from row text </summary>
        /// <param name="text">Row text</param>
        /// 

        public void CreateOryginalInput()
        {
            oryginalInput = "@prefix rdf:   <http://www.w3.org/1999/02/22-rdf-syntax-ns#> .\n" +
            "@prefix xsd:   <http://www.w3.org/2001/XMLSchema#> .\n" +
            "@prefix itsrdf: <http://www.w3.org/2005/11/its/rdf#> .\n" +
            "@prefix nif:   <http://persistence.uni-leipzig.org/nlp2rdf/ontologies/nif-core#> .\n" +
            "@prefix rdfs:  <http://www.w3.org/2000/01/rdf-schema#> .\n\n<" + uri + ">\n" +
            "\t\t" + "a" + "\t\t\t\t" + a + "\n" +
            "\t\t" + "nif:beginIndex" + "\t\"" + beginIndex + "\"^^xsd:nonNegativeInteger ;\n" +
            "\t\t" + "nif:endIndex" + "\t\"" + endIndex + "\"^^xsd:nonNegativeInteger ; \n" +
            "\t\t" + "nif:isString" + "\t\"" + isString + "\"^^xsd:string .\n";

        }
        public Context(String text)
        {
            isString = text;
            beginIndex = 0;
            endIndex = isString.Length;
            a = "nif:RFC5147String , nif:String , nif:Context ;";
            uri = "http://www.ontologydesignpatterns.org/data/oke-challenge-2017/task-2/sentence-1#char=" + beginIndex + "," + endIndex;
        }
        /// <summary> Create Context from file </summary>
        /// <param name="reader">File stream</param>
        public Context(System.IO.StreamReader reader)
        {
            //Get file content
            String fileContent = reader.ReadToEnd();

            //Uri
            String start = "<http://www.ontologydesignpatterns.org";
            String end = ">\r";
      
            int stIndex = fileContent.IndexOf(start);
            int enIndex = fileContent.IndexOf(end);
            if (enIndex == -1)
            {
                enIndex = fileContent.IndexOf(">\n");
            }

            uri = fileContent.Substring(stIndex + 1, enIndex - stIndex -1);

            //a
            a = "nif:RFC5147String , nif:String , nif:Context ;";
            //beginIndex
            start = "nif:beginIndex  \"";
            end = "\"^^xsd:nonNegativeInteger ;";
            stIndex = fileContent.IndexOf(start) + start.Length;
            enIndex = fileContent.IndexOf(end);
            beginIndex = Convert.ToInt32(fileContent.Substring(stIndex, enIndex - stIndex));

            //endIndex
            start = "nif:endIndex    \"";
            stIndex = fileContent.IndexOf(start) + start.Length;
            enIndex = fileContent.LastIndexOf(end);
            endIndex = Convert.ToInt32(fileContent.Substring(stIndex, enIndex - stIndex));

            //isString
            start = "nif:isString";
            end = "\"^^xsd:string";
            string end2 = "@en";
            stIndex = fileContent.IndexOf(start) + start.Length;
            if (fileContent.LastIndexOf(end) != -1)
            {
                enIndex = fileContent.LastIndexOf(end) - stIndex;
            }
            else {
                enIndex = fileContent.LastIndexOf(end2) - stIndex;
            }
            isString = fileContent.Substring(stIndex, enIndex);
            stIndex = isString.IndexOf(" \"") + 2;
            enIndex = isString.Length - stIndex;
            isString = isString.Substring(stIndex, enIndex);
        }
    }

    /// <summary> Fraza powstaje z pliku wejsciowego po przetworzeniu entities i wykryciu klas.</summary>
    public class Phrase : Sentence
    {
        /// <summary> "Captain Boomerang"^^xsd:string ; </summary>
        public String anchorOf;
        /// <summary> Example: <http://www.ontologydesignpatterns.org/data/oke-challenge-2017/task-1/sentence-28#char=0,472> ; </summary>
        public String referenceContext;
        /// <summary> Example: <http://dbpedia.org/resource/Captain_Boomerang> . </summary>
        public String taIdentRef;

        /// <summary> </summary>
        /// <param name="context">Input file Context</param>
        /// <param name="anchorOf">Entity.word</param>
        /// <param name="beginIndex">Entity.begin</param>
        /// <param name="endIndex">Entity.end</param>
        /// <param name="taIdentRef">Link to dbpedia</param>
        public Phrase(Context context, String anchorOf ,int beginIndex, int endIndex, String taIdentRef)
        {
            int enIndex = context.uri.IndexOf("#");
            uri = context.uri.Substring(0, enIndex+1) + "char=" + beginIndex + "," + endIndex;

            a = "nif:RFC5147String , nif:String , nif:Phrase ;";
            this.anchorOf = anchorOf;
            this.beginIndex = beginIndex;
            this.endIndex = endIndex;
            referenceContext = context.uri;
            this.taIdentRef = taIdentRef;
        }
    }


   
}
