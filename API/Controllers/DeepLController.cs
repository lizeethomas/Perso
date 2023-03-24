using Microsoft.AspNetCore.Mvc;
using MyWebsite.Services;
using Newtonsoft.Json;
using System.Diagnostics;
using System.Net;

namespace MyWebsite.Controllers
{
    [Route("deepL")]
    [ApiController]
    public class DeepLController : Controller
    {
        private DeepLService _deepLService;

        public DeepLController(DeepLService deepLService)
        {
            _deepLService = deepLService;
        }


        [HttpPost]
        public IActionResult Translate(string text, string language)
        {
            var translatedText = _deepLService.Translate(text, language);
            var jsonResponse = JsonConvert.DeserializeObject<dynamic>(translatedText);
            Debug.WriteLine($"JSON response: {jsonResponse}");
            if (jsonResponse.translations.Count == 0)
            {
                return BadRequest("No translations found.");
            }
            string textValue = jsonResponse.translations[0]["text"];
            return Ok(textValue);
        }




    }
}


/* 
 
using (var context = new MyDbContext())
{
    var client = context.Clients.Find(1);
 
    // Chargez les commandes associées uniquement lorsque cela est nécessaire
    context.Entry(client).Collection(c => c.Commandes).Load();
}
 
 */