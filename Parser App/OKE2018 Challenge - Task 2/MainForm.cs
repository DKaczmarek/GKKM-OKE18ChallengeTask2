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
        Context context;

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
                                    context = new Context(fileContent);
                                    break;
                                case ".rdf": case ".ttl": case ".xml":
                                    context = new Context(reader);
                                    fileContent = context.isString;
                                    break;
                                default:
                                    MessageBox.Show("Incorrect file extension.", "Error");
                                    Btn_FileOpen_Click(sender, e);
                                    break;
                            }
                        }
                        BtnSaveResults.Enabled = true;
                        InputTextBox.Text = fileContent;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Incorrect file content.", "Error");
                InputTextBox.Text = null;
                BtnSaveResults.Enabled = false;
            }
        }

        private void BtnSaveResults_Click(object sender, EventArgs e)
        {
            try
            {
                Phrase phrase1 = new Phrase(context, "test1", 2, 12, "www.dbpedia.org/test1");
                Phrase phrase2 = new Phrase(context, "test2", 15, 29, "www.dbpedia.org/test2");

                List<Phrase> phrases = new List<Phrase>()
                {
                    phrase1,phrase2
                };

                String output = FileBuilder.BuildOutput(context, phrases);
                using (System.IO.StreamWriter outputFile = new System.IO.StreamWriter(System.IO.Path.Combine(Environment.CurrentDirectory, "ExampleOutput.ttl")))
                {
                    outputFile.WriteLine(output);
                }
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
    }
}
