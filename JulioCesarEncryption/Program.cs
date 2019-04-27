using System;
using System.Security.Cryptography;
using System.Text;

namespace JulioCesarEncryption
{
    class Program
    {
        static void Main(string[] args)
        {
            var encryptedMessage = "jyyjanwcuh cqnan rb wxcqrwp cqjc ljwwxc qjyynw cxmjh. vjat cfjrw";
            var alphabet = new char[26];
            var counter = 0;

            for (var i = 97; i <= 122; i++)
            {
                alphabet[counter] = (char)i;
                counter++;
            }

            var sha1 = CalculateSHA1("Valor", Encoding.Default);

            Console.WriteLine(sha1);
            Console.ReadLine();
        }

        private static string CalculateSHA1(string text, Encoding encoding)
        {
            var buffer = encoding.GetBytes(text);
            var cryptoTransformSha1 = new SHA1CryptoServiceProvider();
            var hash = BitConverter.ToString(cryptoTransformSha1.ComputeHash(buffer)).Replace("-", "");

            return hash;
        }
    }
}
