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
using System.Net.Http;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
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
            async Task<string> Internet(string json)
            {
                var httpClient = new HttpClient();
                var content = new StringContent(json, Encoding.UTF8, "application/json");

                var result = httpClient.PostAsync("http://localhost:17011/crf/predict", content).Result;

                result.EnsureSuccessStatusCode();

                string content_back = await result.Content.ReadAsStringAsync();

                content_back = content_back.Replace("\"", string.Empty);
                JObject JsonResult = JObject.Parse(content_back);
                string word ="";
                string label = "";
                foreach (JProperty x in (JToken)JsonResult)
                {
                    string name = x.Name;
                    JToken value = x.Value;
                    string word_this = value["word"].Value<string>();
                    if (name == "0")
                    {
                        word = word_this;
                        string label_this = value["label"].Value<string>();
                        if (label_this == "O"){ label = label_this; }
                        else {label=label_this.Replace("I-", string.Empty); }

                    }
                    else word =word + "_" + word_this;
                    
                    
                }


                //  JObject jsonObject = JObject.Parse(content2);
                //string a = jsonObject["word"].ToString();
                OutputTextbox.Text = content_back;
                return content_back;
            }

            for (int i = 0; i < jsons.Length; i++) { var a = Internet(jsons[i]); }



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
