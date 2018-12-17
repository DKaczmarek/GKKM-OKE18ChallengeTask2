using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using java.io;
using java.util;
using edu.stanford.nlp.ling;
using edu.stanford.nlp.tagger.maxent;
using edu.stanford.nlp.parser.lexparser;
using Console = System.Console;
using System.Reflection;
using Newtonsoft.Json;
using System.IO;
using System.Text.RegularExpressions;

namespace OKE2018_Challenge___Task_2
{
    public class ParserController
    {
        private const string fileName = "wsj-0-18-bidirectional-nodistsim.tagger";
        private const BindingFlags bindFlags = BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Static;
        private MaxentTagger tagger;
        private Regex regex = new Regex(@"(?!\.)(?!\ )\W");
        public ParserController()
        {
            tagger = new MaxentTagger(Path.Combine(Environment.CurrentDirectory, @"Model\", fileName));
        }

        public List <string> Parse(string text)
        {
            text = regex.Replace(text, " ");

            List<string> JSONs = new List<string>();
            List<Entity> listOfWords = new List<Entity>();
            var sentences = MaxentTagger.tokenizeText(new java.io.StringReader(text)).toArray();
            
            foreach (ArrayList sentence in sentences)
            {
                var taggedSentence = tagger.tagSentence(sentence);
                var pom = taggedSentence.toArray();
                int counter = 0;
                foreach (var x in pom)
                {
                    MemberInfo[] members = x.GetType().GetMembers(bindFlags);
                    var beginF = (MethodInfo)members[15];
                    var endF = (MethodInfo)members[16];
                    var wordF = (MethodInfo)members[9];
                    var tagF = (MethodInfo)members[1];

                    var checkTag = (string)tagF.Invoke(x, null);
                    if (checkTag.Equals("NNP"))
                    {
                        Entity e = new Entity();
                        e.begin = (int)beginF.Invoke(x, null);
                        e.end = (int)endF.Invoke(x, null);
                        e.word = (string)wordF.Invoke(x, null);
                        e.tag = checkTag;
                        if (counter > 0)
                        {
                            int pomCounter = counter - 1;
                            wordBefore pom1 = new wordBefore();
                            pom1.word = (string)wordF.Invoke(pom[pomCounter], null);
                            pom1.tag = (string)tagF.Invoke(pom[pomCounter], null);
                            if (pomCounter == 0)
                                pom1.placeInSentence = "BOS";
                            else
                                pom1.placeInSentence = "MOS";
                            e.wordBefore = pom1;
                        }

                        if (counter < pom.Length - 1)
                        {
                            int pomCounter = counter + 1;
                            wordAfter pom2 = new wordAfter();
                            pom2.word = (string)wordF.Invoke(pom[counter + 1], null);
                            pom2.tag = (string)tagF.Invoke(pom[counter + 1], null);
                            if (pomCounter == pom.Length - 2)
                                pom2.placeInSentence = "EOS";
                            else
                                pom2.placeInSentence = "MOS";

                            if(pom2.word != ".")
                                e.wordAfter = pom2;
                        }

                        if (e.wordAfter.word != null && e.wordBefore.word != null)
                            e.placeInSentence = "MOS";
                        else if (e.wordAfter.word != null && e.wordBefore.word == null)
                            e.placeInSentence = "BOS";
                        else if (e.wordAfter.word == null && e.wordBefore.word != null)
                            e.placeInSentence = "EOS";

                        listOfWords.Add(e);
                    }
                    counter++;
                }
            }

            var pomList = listOfWords.ToArray();
            string JSONresult;
            int flag = 0;
            List<Entity> entities = new List<Entity>();
            for (int i = 0; i < pomList.Length; i++)
            {
                if (i < pomList.Length - 1 && pomList[i].end + 1 == pomList[i + 1].begin)
                {
                    flag = 1;
                    entities.Add(pomList[i]);
                }
                else
                {
                    Dictionary<int, object> pomDictionary = new Dictionary<int, object>();
                    if (flag == 1)
                    {
                        entities.Add(pomList[i]);                        
                        int j = 0;
                        foreach(var val in entities)
                        {
                            pomDictionary.Add(j, val);
                            j++;
                        }
                        JSONresult = JsonConvert.SerializeObject(pomDictionary);
                        JSONresult = JSONresult.Replace(":{\"word\":null,\"tag\":null,\"placeInSentence\":null}", ":{}");
                        JSONs.Add(JSONresult);
                        entities.Clear();
                        pomDictionary.Clear();
                    }
                    else
                    {
                        pomDictionary.Add(0, pomList[i]);
                        JSONresult = JsonConvert.SerializeObject(pomDictionary);
                        JSONresult = JSONresult.Replace(":{\"word\":null,\"tag\":null,\"placeInSentence\":null}", ":{}");
                        JSONs.Add(JSONresult);
                        pomDictionary.Clear();
                    }
                    flag = 0;
                }
            }

            return JSONs;
        }

    }
}
