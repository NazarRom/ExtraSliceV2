using System.Security.Cryptography;
using System.Text;

namespace ExtraSliceV2.Helpers
{
    public class HelperCryptograhy
    {
        public static string GenerateSalt()
        {
            //tendremos un salt de 50
            Random random = new Random();//hago un numero random 
            string salt = ""; //esta variable es la que voy a devolver y combinar con la contraseña
            for (int i = 1; i <= 50; i++) //bucle que va de 0 a 50
            {
                int aleat = random.Next(0, 255); //un numero random entre 0 y 255
                char letra = Convert.ToChar(aleat); //convierto ese numero a letra
                salt += letra;//concatenas el salt con el char random
            }
            return salt;//devulevo salt
        }

        //en algun momento tenemos que comparar si los pass son iguales

        public static bool CompareArray(byte[] a, byte[] b)
        {
            bool iguales = true;
            if (a.Length != b.Length)
            {
                iguales = false;
            }else
            {
                for (int i = 0;i < a.Length; i++)
                {
                    if (a[i].Equals(b[i]) == false)
                    {
                        iguales = false;
                        break;
                    }
                }
            }
            return iguales;

        }

        //tendremos un metodo para cifrar nuestro password

        public static byte[] EncryptPassword(
            string password, string salt)
        {
            string contenido = password + salt;
            SHA512 sHA = SHA512.Create();
            //convertimos buestro cotenido a byte
            byte[] salida = Encoding.UTF8.GetBytes(contenido);
            //iteraciones para nuestro password
            for (int i = 0; i <= 107; i++) 
            {
                salida = sHA.ComputeHash(salida);
            }
            sHA.Clear();
            return salida;
        }

    }
}

