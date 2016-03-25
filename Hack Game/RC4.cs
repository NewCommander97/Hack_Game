using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hack_Game
{
    [Serializable()]
    class RC4
    {
        public static byte[] Crypt(byte[] bytes, byte[] key)
        {
            byte[] s = new byte[256];
            byte[] k = new byte[256];
            byte temp;
            int i, j;

            for (i = 0; i < 256; i++)
            {
                s[i] = (byte)i;
                k[i] = key[i % key.GetLength(0)];
            }

            j = 0;
            for (i = 0; i < 256; i++)
            {
                j = (j + s[i] + k[i]) % 256;
                temp = s[i];
                s[i] = s[j];
                s[j] = temp;
            }

            i = j = 0;
            for (int x = 0; x < bytes.GetLength(0); x++)
            {
                i = (i + 1) % 256;
                j = (j + s[i]) % 256;
                temp = s[i];
                s[i] = s[j];
                s[j] = temp;
                int t = (s[i] + s[j]) % 256;
                bytes[x] ^= s[t];
            }
            return bytes;
        }

        public static byte[] Crypt(byte[] bytes, string key)
        {
            byte[] ret = Crypt(bytes, Encoding.ASCII.GetBytes(key));
            return ret;
        }

        public static byte[] CryptToBytes(string input, string key)
        {
            byte[] ret = Crypt(Encoding.ASCII.GetBytes(input), Encoding.ASCII.GetBytes(key));
            return ret;
        }

        public static string CryptToString(string input, string key)
        {
            byte[] ret = Crypt(Encoding.ASCII.GetBytes(input), Encoding.ASCII.GetBytes(key));
            return Encoding.ASCII.GetString(ret);
        }
    }
}
