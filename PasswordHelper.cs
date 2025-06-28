using System;
using System.Security.Cryptography;
using System.Text;

namespace ZimVaxSync
{
    public class PasswordHelper
    {
        // Method to hash the password using SHA256
        public static string HashPassword(string password)
        {
            using (SHA256 sha256 = SHA256.Create())
            {
                // Convert password string to byte array
                byte[] passwordBytes = Encoding.UTF8.GetBytes(password);

                // Hash the password bytes
                byte[] hashedBytes = sha256.ComputeHash(passwordBytes);

                // Convert the hashed bytes back to a hex string
                StringBuilder hashedPassword = new StringBuilder();
                foreach (byte b in hashedBytes)
                {
                    hashedPassword.Append(b.ToString("x2"));
                }

                return hashedPassword.ToString();
            }
        }
    }
}
