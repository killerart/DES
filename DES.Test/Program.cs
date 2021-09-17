using System;

namespace DES.Test {
    class Program {
        static void Main() {
            // Console.Write("Input encryption key: ");
            // var key = Console.ReadLine();
            var key = "DESkey56";
            // Console.Write("Input message to encrypt: ");
            // var message          = Console.ReadLine();
            var message          = "qwerty";
            var encryptedMessage = DES.Encrypt(message, key);
            Console.WriteLine($"Encrypted message: {Convert.ToHexString(encryptedMessage)}");
            var decryptedMessage = DES.Decrypt(encryptedMessage, key);
            Console.WriteLine($"Decrypted message: {decryptedMessage}");
        }
    }
}
