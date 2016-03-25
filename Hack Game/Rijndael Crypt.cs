using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Security.Cryptography;

namespace Hack_Game
{
    [Serializable()]
    class Rijndael_Crypt
    {
        public static byte[] Key { get; set; }

        public static byte[] IV { get; set; }

        public static byte[] GenerateKeyStatic(int keySize)
        {
            if (keySize > 256 || keySize < 0)
                return null;
            Rijndael AESCrypto = Rijndael.Create();
            AESCrypto.KeySize = keySize;
            AESCrypto.GenerateKey();
            Key = AESCrypto.Key;
            return Key;
        }

        public static byte[] GenerateIVStatic()
        {
            Rijndael AESCrypto = Rijndael.Create();
            AESCrypto.GenerateIV();
            IV = AESCrypto.IV;
            return IV;
        }

        public byte[] GenerateKey(int keySize)
        {
            if (keySize > 256 || keySize < 0)
                return null;
            Rijndael AESCrypto = Rijndael.Create();
            AESCrypto.KeySize = keySize;
            AESCrypto.GenerateKey();
            Key = AESCrypto.Key;
            return Key;
        }

        public byte[] GenerateIV()
        {
            Rijndael AESCrypto = Rijndael.Create();
            AESCrypto.GenerateIV();
            IV = AESCrypto.IV;
            return IV;
        }

        public byte[] Decode(byte[] encryptedBytes)
        {
            Rijndael AESCrypto = Rijndael.Create();
            AESCrypto.Key = Key;
            AESCrypto.IV = IV;

            MemoryStream ms = new MemoryStream();
            CryptoStream cs = new CryptoStream(ms, AESCrypto.CreateDecryptor(), CryptoStreamMode.Write);

            cs.Write(encryptedBytes, 0, encryptedBytes.Length);
            cs.Close();

            return ms.ToArray();
        }

        public byte[] Decode(byte[] encryptedBytes, byte[] key, byte[] iv)
        {
            Rijndael AESCrypto = Rijndael.Create();
            AESCrypto.Key = key;
            AESCrypto.IV = iv;

            MemoryStream ms = new MemoryStream();
            CryptoStream cs = new CryptoStream(ms, AESCrypto.CreateDecryptor(), CryptoStreamMode.Write);

            cs.Write(encryptedBytes, 0, encryptedBytes.Length);
            cs.Close();

            return ms.ToArray();
        }

        public byte[] Encode(byte[] decryptedBytes)
        {
            Rijndael AESCrypto = Rijndael.Create();
            AESCrypto.Key = Key;
            AESCrypto.IV = IV;

            MemoryStream ms = new MemoryStream();
            CryptoStream cs = new CryptoStream(ms, AESCrypto.CreateEncryptor(), CryptoStreamMode.Write);

            cs.Write(decryptedBytes, 0, decryptedBytes.Length);
            cs.Close();

            return ms.ToArray();
        }

        public byte[] Encode(byte[] decryptedBytes, byte[] key, byte[] iv)
        {
            Rijndael AESCrypto = Rijndael.Create();
            AESCrypto.Key = key;
            AESCrypto.IV = iv;

            MemoryStream ms = new MemoryStream();
            CryptoStream cs = new CryptoStream(ms, AESCrypto.CreateEncryptor(), CryptoStreamMode.Write);

            cs.Write(decryptedBytes, 0, decryptedBytes.Length);
            cs.Close();

            return ms.ToArray();
        }
    }
}