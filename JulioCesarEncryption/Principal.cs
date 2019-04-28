using System;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using Newtonsoft.Json;

namespace JulioCesarEncryption
{
    class Principal
    {
        public const string CAMINHO_ARQUIVO = "../../answer.json";

        static void Main(string[] args)
        {
            var mensagem = LoadJson();

            mensagem.Decifrado = EncryptMessage(mensagem.Cifrado, mensagem.NumeroCasas);
            mensagem.ResumoCriptografico = ConvertSha1(mensagem.Decifrado, Encoding.Default);

            UpdateJson(mensagem);

            Console.ReadLine();
        }

        private static string EncryptMessage(string mensagem, int numeroCasas)
        {
            var alfabeto = GenerateAlphabet(numeroCasas);
            var caracteresMensagem = mensagem.ToCharArray();
            var caracteresDecriptados = new char[mensagem.Length];

            for (var i = 0; i < caracteresMensagem.Length; i++)
                if (alfabeto.LetrasEncriptadas.Contains(caracteresMensagem[i]))
                    caracteresDecriptados[i] = alfabeto.LetrasOriginais[Array.IndexOf(alfabeto.LetrasEncriptadas, caracteresMensagem[i])];
                else
                    caracteresDecriptados[i] = caracteresMensagem[i];
     
            return new string(caracteresDecriptados);
        }

        private static Alfabeto GenerateAlphabet(int numeroCasas)
        {
            Alfabeto alfabeto = new Alfabeto();
            var indexLetra = 0; 

            for (var i = 97; i <= 122; i++)
            {
                int codigoLetraAuxiliar;

                if (i + numeroCasas > 122)
                    codigoLetraAuxiliar = (i + numeroCasas - 122 - 1) + 97;
                else
                    codigoLetraAuxiliar = i + numeroCasas;

                alfabeto.LetrasOriginais[indexLetra] = (char)i;
                alfabeto.LetrasEncriptadas[indexLetra] = (char)codigoLetraAuxiliar;

                indexLetra++;
            }

            return alfabeto;
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
            using (StreamReader r = new StreamReader(CAMINHO_ARQUIVO))
            {
                var json = r.ReadToEnd();
                Mensagem mensagem = JsonConvert.DeserializeObject<Mensagem>(json);

                return mensagem;
            }
        }

        private static void UpdateJson(Mensagem mensagem)
        {
            string output = JsonConvert.SerializeObject(mensagem, Formatting.Indented);
            File.WriteAllText(CAMINHO_ARQUIVO, output);
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

    public class Alfabeto
    {
        public char[] LetrasOriginais { get; set; }
        public char[] LetrasEncriptadas { get; set; }

        public Alfabeto()
        {
            LetrasOriginais = new char[26];
            LetrasEncriptadas = new char[26];
        }
    }
}
