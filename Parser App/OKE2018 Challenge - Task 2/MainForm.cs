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

            // Loading english PCFG parser from file
            var lp = LexicalizedParser.loadModel(Environment.CurrentDirectory + @"\englishPCFG.ser");

            // This option shows loading and using an explicit tokenizer
            var sent2 = InputTextBox.Text;
            var tokenizerFactory = PTBTokenizer.factory(new CoreLabelTokenFactory(), "");
            var sent2Reader = new StringReader(sent2);
            var rawWords2 = tokenizerFactory.getTokenizer(sent2Reader).tokenize();
            sent2Reader.close();
            var tree2 = lp.apply(rawWords2);

            // Extract dependencies from lexical tree
            var tlp = new PennTreebankLanguagePack();
            var gsf = tlp.grammaticalStructureFactory();
            var gs = gsf.newGrammaticalStructure(tree2);
            var tdl = gs.typedDependenciesCCprocessed();
            Console.WriteLine("\n{0}\n", tdl);
            //OutputTextbox.Text = tdl.ToString();

            // Extract collapsed dependencies from parsed tree
            var tp = new TreePrint("penn,typedDependenciesCollapsed");
            tp.printTree(tree2);
            OutputTextbox.Text = tree2.ToString();
        }

        private void Btn_FileOpen_Click(object sender, EventArgs e)
        {
            var fileContent = string.Empty;
            var filePath = string.Empty;

            using (OpenFileDialog openFileDialog = new OpenFileDialog())
            {
                openFileDialog.InitialDirectory = Environment.CurrentDirectory;
                openFileDialog.Filter = "txt files (*.txt)|*.txt|rdf files(*.rdf)|*.rdf|All files (*.*)|*.*";
                openFileDialog.FilterIndex = 2;
                openFileDialog.RestoreDirectory = true;

                if (openFileDialog.ShowDialog() == DialogResult.OK)
                {
                    filePath = openFileDialog.FileName;
                    var fileStream = openFileDialog.OpenFile();

                    using (System.IO.StreamReader reader = new System.IO.StreamReader(fileStream))
                    {
                        fileContent = reader.ReadToEnd();
                    }
                }
            }
            InputTextBox.Text = fileContent;
        }
    }
}
