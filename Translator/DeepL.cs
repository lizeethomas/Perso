using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace Translator
{
    public class DeepL
    {
        private readonly HttpClient _httpClient;
        private readonly string _authKey;
        private delegate string EditString(string text);    

        public DeepL()
        {
            _httpClient = new HttpClient();
            _httpClient.BaseAddress = new Uri("https://api-free.deepl.com/v2/");
            _authKey = "1d4e0bd2-7922-de6d-6449-e4248bf03b42:fx";
        }

        public string Translate(string text, string targetLang)
        {
            var requestContent = new FormUrlEncodedContent(new[]
            {
                new KeyValuePair<string, string>("text", text),
                new KeyValuePair<string, string>("target_lang", targetLang)
            });

            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("DeepL-Auth-Key", _authKey);
            _httpClient.DefaultRequestHeaders.UserAgent.ParseAdd("YourApp/1.2.3");

            var response = _httpClient.PostAsync("translate", requestContent).Result;
            if (response != null)
            {
                return response.Content.ReadAsStringAsync().Result;
            }

            return "";
        }

        public string setToLowerCase(string text)
        {
            return text.ToLower();
        }
        public string setToUpperCase (string text)
        {
            return text.ToUpper(); 
        }
    }
}
