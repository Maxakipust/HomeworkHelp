using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Sodium;

namespace Encryption
{
    public class Encryptor
    {
        public static KeyPair GenerateKeyPair()
        {
            return PublicKeyBox.GenerateKeyPair();
        }
        
        public static byte[] Encrypt(string message, KeyPair keypair)
        {
            var nonce = PublicKeyBox.GenerateNonce();
            var cipher = PublicKeyBox.Create(message, nonce, keypair.PrivateKey, keypair.PublicKey);
            var output = new byte[nonce.Length + cipher.Length];
            nonce.CopyTo(output, 0);
            cipher.CopyTo(output, cipher.Length);
            return output;
        }

        public static string Decrypt(byte[] cipherText, KeyPair keypair)
        {
            var nonce = new byte[24];
            var cipher = new byte[cipherText.Length - 24];
            Array.Copy(cipherText, nonce, 24);
            Array.Copy(cipherText, 24, cipher, 0, cipherText.Length - 24);
            return PublicKeyBox.Open(cipher, nonce, keypair.PrivateKey, keypair.PublicKey).ToString();
        }
    }
}
