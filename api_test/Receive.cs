using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace api_test
{
    class Sentence
    {
        [JsonProperty("fname")]
        public string Film { get; set; }

        [JsonProperty("en")]
        public string English { get; set; }

        [JsonProperty("cn")]
        public string Chinese { get; set; }

        [JsonProperty("voice")]
        public string Voice { get; set; }
    }



    class Receive
    {
        [JsonProperty("statue")]
        public int StatueCode { get; set; }

        [JsonProperty("word")]
        public string Word { get; set; }

        [JsonProperty("trans")]
        public string Translate { get; set; }

        [JsonProperty("sentences")]
        public List<Sentence> Sentences { get; set; }
    }
}
