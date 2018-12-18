using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OKE2018_Challenge___Task_2
{
    /// <summary>
    /// Gets all results and creates complete string
    /// </summary>
    public static class FileBuilder
    {
        public static String BuildOutput(Context context, List<Phrase> phrases)
        {
            StringBuilder builder = new StringBuilder();
            builder.Append(context.oryginalInput);

            if (phrases != null)
            {
                foreach (var phrase in phrases)
                {
                    builder.Append("\n<" + phrase.uri + ">\n");
                    builder.Append("\t\t" + "a" + "\t\t\t\t\t\t" + phrase.a + "\n");
                    builder.Append("\t\t" + "nif:anchorOf" + "\t\t\t\"" + phrase.anchorOf + "\"^^xsd:string ;\n");
                    builder.Append("\t\t" + "nif:beginIndex" + "\t\t\t\"" + phrase.beginIndex + "\"^^xsd:nonNegativeInteger ;\n");
                    builder.Append("\t\t" + "nif:endIndex" + "\t\t\t\"" + phrase.endIndex + "\"^^xsd:nonNegativeInteger ;\n");
                    builder.Append("\t\t" + "nif:referenceContext" + "\t<" + context.uri + "> ;\n");
                    builder.Append("\t\t" + "itsrdf:taIdentRef" + "\t\t<" + phrase.taIdentRef + "> .\n");
                }
            }
            return builder.ToString();
        }
    }
}
