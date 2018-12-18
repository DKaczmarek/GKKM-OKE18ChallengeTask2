using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using java.io;
using edu.stanford.nlp.process;
using edu.stanford.nlp.ling;
using edu.stanford.nlp.trees;
using edu.stanford.nlp.parser.lexparser;
using Console = System.Console;

namespace OKE2018_Challenge___Task_2
{
    public partial class Main_Form : Form
    {
        public Main_Form()
        {
            InitializeComponent();
        }

        private void Btn_Parse_Click(object sender, EventArgs e)
        {

            var sent2 = InputTextBox.Text;

            ParserController parser = new ParserController();
            var jsons = parser.Parse(sent2).ToArray();
            OutputTextbox.Text = jsons[0];
        }

        private void Btn_FileOpen_Click(object sender, EventArgs e)
        {
            Sentence sentence = new Sentence("", "", "", 1, 2, "", "");

            String fileContent, filePath, filextension;
            fileContent = filePath = filextension = String.Empty;
            try
            {
                using (OpenFileDialog openFileDialog = new OpenFileDialog())
                {
                    openFileDialog.InitialDirectory = Environment.CurrentDirectory;
                    openFileDialog.Filter = "txt files (*.txt)|*.txt|ttl files (*.ttl)|*.ttl|rdf files(*.rdf)|*.rdf|xml files(*.xml)|*.xml|All files (*.*)|*.*";
                    openFileDialog.FilterIndex = 2;
                    openFileDialog.RestoreDirectory = true;

                    if (openFileDialog.ShowDialog() == DialogResult.OK)
                    {
                        filextension = System.IO.Path.GetExtension(openFileDialog.FileName);
                        filePath = openFileDialog.FileName;
                        var fileStream = openFileDialog.OpenFile();

                        using (System.IO.StreamReader reader = new System.IO.StreamReader(fileStream))
                        {
                            switch (filextension)
                            {
                                case ".txt":
                                    fileContent = reader.ReadToEnd();
                                    break;
                                case ".rdf":
                                    fileContent = File_Parse(reader.ReadToEnd());
                                    break;
                                case ".ttl":
                                    fileContent = File_Parse(reader.ReadToEnd());
                                    break;
                                case ".xml":
                                    fileContent = File_Parse(reader.ReadToEnd());
                                    break;
                                default:
                                    MessageBox.Show("Incorrect file extension.", "Error");
                                    Btn_FileOpen_Click(sender, e);
                                    break;
                            }
                        }

                        InputTextBox.Text = fileContent;
                    }
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show("Incorrect file content.", "Error");
                Btn_FileOpen_Click(sender, e);
            }
        }

        private String File_Parse(String fileContent)
        {
            String isStringStart = "nif:isString";
            String isStringEnd = "\"^^xsd:string";
            int parseFrom = fileContent.IndexOf(isStringStart) + isStringStart.Length;
            int parseTo = fileContent.LastIndexOf(isStringEnd) - parseFrom;
            String isString = fileContent.Substring(parseFrom, parseTo);

            parseFrom = isString.IndexOf(" \"") + 2;
            parseTo = isString.Length - parseFrom;
            isString = isString.Substring(parseFrom, parseTo);
            return isString;
        }

        public class Sentence
        {
            /// <summary>
            /// <http://www.ontologydesignpatterns.org/data/oke-challenge-2017/task-1/sentence-28#char=303,320>
            /// </summary>
            public String uri;
            /// <summary>
            ///  nif:RFC5147String , nif:String , nif:Phrase ;
            /// </summary>
            public String a;
            /// <summary>
            /// "Captain Boomerang"^^xsd:string ;
            /// </summary>
            public String anchorOf;
            /// <summary>
            /// "303"^^xsd:nonNegativeInteger ;
            /// </summary>
            public String beginIndex;
            /// <summary>
            /// "320"^^xsd:nonNegativeInteger ;
            /// </summary>
            public String endIndex;
            /// <summary>
            /// <http://www.ontologydesignpatterns.org/data/oke-challenge-2017/task-1/sentence-28#char=0,472> ;
            /// </summary>
            public String referenceContext;
            /// <summary>
            /// <http://dbpedia.org/resource/Captain_Boomerang> .
            /// </summary>
            public String taIdentRef;

            public Sentence(String uri, String a, String anchorOf, int beginIndex, int endIndex, String referenceContext, String taIdentRef)
            {
                StringBuilder sb = new StringBuilder();
                this.beginIndex = sb.Append("\"").Append(beginIndex).Append("\"^^xsd:nonNegativeInteger ;").ToString();
                this.endIndex = sb.Clear().Append("\"").ToString();
            }
        }
    }
}
