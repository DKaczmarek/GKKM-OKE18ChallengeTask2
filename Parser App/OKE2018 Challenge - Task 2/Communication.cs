using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Http;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace OKE2018_Challenge___Task_2
{
    class Communication
    {
        public string[] jsons;

        public Communication(string[] js)
        {
            this.jsons = js;
        }

        async public void Communicate()
        {
            for (int i = 0; i < jsons.Length; i++)
            {
                JSON_entity js = await Internet(jsons[i]);
                if (js.label != "O")
                {
                    
                    SPARQL sparql = new SPARQL(js);
                    sparql.ask();
                }
            }
        }

        async Task<JSON_entity> Internet(string json)
        {
            var httpClient = new HttpClient();
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            var result = httpClient.PostAsync("http://localhost:17011/crf/predict", content).Result;

            result.EnsureSuccessStatusCode();

            string content_back = await result.Content.ReadAsStringAsync();

            content_back = content_back.Replace("\"", string.Empty);
            JObject JsonResult = JObject.Parse(content_back);
            string word = "";
            string label = "";
            JSON_entity js = new JSON_entity();
            foreach (JProperty x in (JToken)JsonResult)
            {
                string name = x.Name;
                JToken value = x.Value;
                string word_this = value["word"].Value<string>();
                if (name == "0")
                {
                    int begin = value["begin"].Value<int>();
                    js.begin_index = begin;
                    if (name == (JsonResult.Count - 1).ToString())
                    {
                        int end = value["end"].Value<int>();
                        js.end_index = end;
                        js.name = word_this;
                    }
                    word = word_this;
                    string label_this = value["label"].Value<string>();
                    if (label_this == "O") { label = label_this; js.label = label; }
                    else { label = label_this.Replace("I-", string.Empty); label = label.Replace("O-", string.Empty); js.label = label; }

                }
                else
                {
                    word = word + " " + word_this;
                    js.name = word;
                    if (name == (JsonResult.Count - 1).ToString())
                    {
                        int end = value["end"].Value<int>();
                        js.end_index = end;
                    }
                }


            }

            return js;
        }

            
    }

}

