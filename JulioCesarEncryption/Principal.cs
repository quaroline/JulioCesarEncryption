using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using Newtonsoft.Json;

namespace JulioCesarEncryption
{
    class Principal
    {
        static void Main(string[] args)
        {
            var mensagem = LoadJson();

            mensagem.ResumoCriptografico = ConvertSha1("Valor", Encoding.Default);
            
            Console.ReadLine();
        }

        private static string EncryptMessage()
        {
            return "Teste inicial.";
        }

        private static char[] GenerateAlphabet()
        {
            var alphabet = new char[26];
            var counter = 0;

            for (var i = 97; i <= 122; i++)
            {
                alphabet[counter] = (char)i;
                counter++;
            }

            return alphabet;
        }

        private static string ConvertSha1(string text, Encoding encoding)
        {
            var buffer = encoding.GetBytes(text);
            var cryptoTransformSha1 = new SHA1CryptoServiceProvider();
            var hash = BitConverter.ToString(cryptoTransformSha1.ComputeHash(buffer)).Replace("-", "");

            return hash;
        }

        private static Mensagem LoadJson()
        {
            using (StreamReader r = new StreamReader("../../answer.json"))
            {
                var json = r.ReadToEnd();
                Mensagem mensagem = JsonConvert.DeserializeObject<Mensagem>(json);
                return mensagem;
            }
        }
    }

    public class Mensagem
    {
        public string Token { get; set; }

        [JsonProperty("numero_casas")]
        public int NumeroCasas { get; set; }

        public string Cifrado { get; set; }

        public string Decifrado { get; set; }

        [JsonProperty("resumo_criptografico")]
        public string ResumoCriptografico { get; set; }
    }
}
