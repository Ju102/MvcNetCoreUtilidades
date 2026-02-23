using System.Security.Cryptography;
using System.Text;

namespace MvcNetCoreUtilidades.Helpers
{
    public class HelperCryptography
    {
        // Creamos los métodos de tipo static

        // Creamos un string para el SALT
        public static string Salt { get; set; }

        // Metodo para generar un SALT aleatorio
        private static string GenerateSalt()
        {
            Random random = new Random();
            string salt = "";

            for (int i = 1; i <= 30; i++)
            {
                int num = random.Next(1, 255);
                char letra = Convert.ToChar(num);
                salt += letra;
            }

            return salt;
        }

        public static string CifrarContenidoSalt(string contenido, bool comparar)
        {
            if (comparar == false)
            {
                Salt = GenerateSalt();
            }

            // Realizamos el cifrado.
            string contenidoSalt = contenido + Salt;
            // Utilizamos el objeto grande para cifrar.
            SHA512 managed = SHA512.Create();
            UnicodeEncoding encoding = new UnicodeEncoding();
            byte[] salida = encoding.GetBytes(contenidoSalt);

            for (int i = 1; i <= 21; i++)
            {
                // Cifrado sobre cifrado.
                salida = managed.ComputeHash(salida);
            }
            // Se debe liberar la memoria
            managed.Clear();

            string resultado = encoding.GetString(salida);

            return resultado;
        }

        // Se devuelve un texto cifrado simple
        public static string EncriptarTextoBasico(string contenido)
        {
            // El cifrado se realiza a nivel de bytes.
            // Se debe convertir el texto de entrada a bytes[].
            byte[] entrada;

            // Cuando se cifre, se dará una salida de bytes[].
            byte[] salida;

            // Se necesita una clase para convertir de string a byte[] y viceversa.
            UnicodeEncoding encoding = new UnicodeEncoding();

            // También se necesita un objeto para cifrar el contenido.
            SHA1 managed = SHA1.Create();

            // Convertimos el texto a bytes.
            entrada = encoding.GetBytes(contenido);

            // Los objetos de cifrado tienen un método llamado ComputeHash(), que recibe un array de bytes,
            // realiza acciones internas y devuelve un array de bytes cifrado.
            salida = managed.ComputeHash(entrada);

            // Se convierten los bytes a texto.
            string resultado = encoding.GetString(salida);

            return resultado;
        }
    }
}
