using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace api_test
{
    class Security
    {
        public Security(string userName, string key)
        {
            this.UserName = userName;
            DateTime startTime = TimeZone.CurrentTimeZone.ToLocalTime(new DateTime(1970, 1, 1));
            this.Time = Math.Round((DateTime.Now - startTime).TotalSeconds, 4);
            this.Key = CreateMD5(key+Time);
        }

        private static string CreateMD5(string input)
        {
            // Use input string to calculate MD5 hash
            using (System.Security.Cryptography.MD5 md5 = System.Security.Cryptography.MD5.Create())
            {
                byte[] inputBytes = System.Text.Encoding.UTF8.GetBytes(input);
                byte[] hashBytes = md5.ComputeHash(inputBytes);

                // Convert the byte array to hexadecimal string
                StringBuilder sb = new StringBuilder();
                for (int i = 0; i < hashBytes.Length; i++)
                {
                    sb.Append(hashBytes[i].ToString("X2"));
                }
                return sb.ToString().ToLower();
            }
        }


        [JsonProperty("uname")]
        public string UserName { get; set; }

        [JsonProperty("key")]
        public string Key { get; set; }

        [JsonProperty("time")]
        public double Time { get; set; }
    }

    class Option
    {
        public Option(int level,int count)
        {
            this.Level = level;
            this.Count = count;
        }

        [JsonProperty("level")]
        public int Level { get; set; }

        [JsonProperty("count")]
        public int Count { get; set; }
    }

    class Send
    {
        public Send(string userName, string key, string word, int level,int count)
        {
            this.Security = new Security(userName,key);
            this.Option = new Option(level,count);
            this.Word = word;
        }

        [JsonProperty("security")]
        public Security Security { get; set; }

        [JsonProperty("word")]
        public string Word { get; set; }

        [JsonProperty("option")]
        public Option Option { get; set; }
    }
}
