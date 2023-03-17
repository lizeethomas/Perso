
using static System.Net.Mime.MediaTypeNames;
using System.Diagnostics;
using Translator;
using Newtonsoft.Json;

string language; string text;
string exit = "";

Console.WriteLine("DEEPL CUSTOM (appuyez deux fois sur Entrée pour sortir)");

do
{
    try
    {
        Console.WriteLine("Entrez la langue souhaitée au format 'XX' : ");
        language = Console.ReadLine();
        Console.WriteLine("Entrez le mot ou texte à traduire: ");
        text = Console.ReadLine();

        var _deepL = new DeepL();
        var translatedText = _deepL.Translate(text, language);

        Debug.WriteLine(translatedText);
        var jsonResponse = JsonConvert.DeserializeObject<dynamic>(translatedText);
        string? textValue = jsonResponse?.translations[0]["text"];
        Console.WriteLine($"\n{textValue}\n");

    }
    catch
    {
        Console.WriteLine("Erreur dans la déclaration des entrées");
        exit = "exit";
    }
} while (exit != "exit");




