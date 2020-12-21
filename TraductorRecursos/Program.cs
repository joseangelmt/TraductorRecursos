using System;
using System.IO;
using Newtonsoft.Json;
using RestSharp;

namespace TraductorRecursos
{
    internal struct DatosTraduccion
    {
        public string text { get; set; }
    }

    internal class Respuesta
    {
        public DatosTraduccion[] translations { get; set; }
    }

    internal class Program
    {
        private static void Main(string[] args)
        {
            if (args.Length < 3)
            {
                var oldColor = Console.ForegroundColor;
                Console.ForegroundColor = ConsoleColor.Red;
                Console.Error.WriteLine("Error: Tienes que especificar los parámetros [ruta archivo .ini] [clave de autenticación] [Código idioma a traducir]");
                Console.ForegroundColor = oldColor;
                return;
            }

            if (!File.Exists(args[0]))
            {
                Console.Error.WriteLine($"El archivo {args[0]} no existe");
                return;
            }

            TraduceArchivo(args[0], args[1], args[2]);
        }

        private static void TraduceArchivo(string rutaArchivo, string clave, string idiomaDestino)
        {
            using (var archivo = File.Open(rutaArchivo, FileMode.Open, FileAccess.Read))
            using (var sr = new StreamReader(archivo))
            {
                while (!sr.EndOfStream)
                {
                    var linea = sr.ReadLine();
                    if (linea == null) continue;

                    var separador = linea.IndexOf("\"=\"", StringComparison.Ordinal);
                    if (-1 == separador)
                    {
                        Console.WriteLine(linea);
                        continue;
                    }

                    var izquierda = linea.Substring(1, separador - 1);
                    var derecha = Traduce(izquierda, clave, idiomaDestino);
                    Console.WriteLine($"\"{izquierda}\"=\"{derecha}\"");
                }
            }
        }

        public static string Traduce(string cadena, string clave, string idiomaDestino)
        {
            var client = new RestClient("https://api.deepl.com/v2/translate");
            var request = new RestRequest(Method.POST);
            request.AddParameter("auth_key", clave);
            request.AddParameter("text", cadena);
            request.AddParameter("source_lang", "EN");
            request.AddParameter("target_lang", idiomaDestino);
            var respuesta = client.Execute(request);

            var objeto = JsonConvert.DeserializeObject<Respuesta>(respuesta.Content);
            return objeto.translations[0].text;
        }
    }
}
