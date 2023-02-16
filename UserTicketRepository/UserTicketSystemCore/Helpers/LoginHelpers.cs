using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using UserTicketSystemCore.Models;
using UserTicketSystemCore.Models.Dtos;

namespace UserTicketSystemCore.Helpers
{
    public static class LoginHelpers
    {
        public static bool VerifyPasswordHash(string password, byte[] passwordHash, byte[] passwordSalt)
        {
            using (var hmac = new HMACSHA512(passwordSalt))
            {
                var computedHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(password));
                for (int i = 0; i < computedHash.Length; i++)
                {
                    if (computedHash[i] != passwordHash[i]) return false;
                }
            }
            return true;
        }

        public static void AddHashAndSalt(User user, LoginDto userDto) 
        {
            byte[] salt = new byte[32];
            using (var rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(salt);
            }

            // Hash the password
            byte[] passwordBytes = Encoding.UTF8.GetBytes(userDto.Password);
            byte[] saltedPasswordBytes = passwordBytes.Concat(salt).ToArray();
            byte[] hashedPasswordBytes;
            using (var sha256 = SHA256.Create())
            {
                hashedPasswordBytes = sha256.ComputeHash(saltedPasswordBytes);
            }

            user.PasswordHash = hashedPasswordBytes;
            user.PasswordSalt = salt;
        }

        public static string GenerateJwtSecret(int length = 32)
        {
            var rng = new RNGCryptoServiceProvider();
            var bytes = new byte[length];
            rng.GetBytes(bytes);
            return Convert.ToBase64String(bytes);
        }
    }
}
