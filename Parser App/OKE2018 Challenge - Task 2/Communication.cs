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
                string[] a = await Internet(jsons[i]);
                if (a.Length == 2)
                {
                    SPARQL sparql = new SPARQL(a[0], a[1]);
                    sparql.ask();
                }
            }
        }

        async Task<string[]> Internet(string json)
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
            foreach (JProperty x in (JToken)JsonResult)
            {
                string name = x.Name;
                JToken value = x.Value;
                string word_this = value["word"].Value<string>();
                if (name == "0")
                {
                    word = word_this;
                    string label_this = value["label"].Value<string>();
                    if (label_this == "O") { label = label_this; }
                    else { label = label_this.Replace("I-", string.Empty); }

                }
                else word = word + " " + word_this;


            }
            var pomList = new List<string>();

            // Add items to the list
            pomList.Add(word);
            if(label != "O")pomList.Add(label);

            // Convert to array
            var return_s = pomList.ToArray();
            return return_s;
        }

            
    }

}

