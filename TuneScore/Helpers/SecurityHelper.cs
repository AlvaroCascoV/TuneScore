using System.Security.Cryptography;
using System.Text;

namespace TuneScore.Helpers
{
    public static class SecurityHelper
    {
        public static string GenerateSalt()
        {
            Random random = new Random();
            string salt = "";
            for (int i = 0; i < 50; i++)
            {
                int num = random.Next(1, 255);
                char letra = Convert.ToChar(num);
                salt += letra;
            }
            return salt;
        }

        public static bool CompareArrays(byte[] array1, byte[] array2)
        {
            bool iguales = true;
            if (array1.Length != array2.Length)
            {
                iguales = false;
            }
            else
            {
                for (int i = 0; i < array1.Length; i++)
                {
                    if (array1[i] != array2[i])
                    {
                        iguales = false;
                        break;
                    }
                }
            }
            return iguales;
        }
        public static byte[] EncryptPassword(string password, string salt)
        {
            string contenido = password + salt;
            SHA512 managed = SHA512.Create();
            byte[] salida = Encoding.UTF8.GetBytes(contenido);
            for (int i = 0; i <= 15; i++)
            {
                salida = managed.ComputeHash(salida);
            }
            managed.Clear();
            return salida;
        }
    }
}

