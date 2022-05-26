using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Threading.Tasks;

namespace API.Services
{
    public class EncryptionService : IEncryptionService
    {
        public HashSalt EncryptPassword(string password)
        {
            byte[] salt = new byte[128 / 8]; //Generate a 128 bit salt 
            using (var rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(salt);
            }
            string encryptedPassword = Convert.ToBase64String(KeyDerivation.Pbkdf2(
                password: password,
                salt: salt,
                prf: KeyDerivationPrf.HMACSHA1,
                iterationCount: 10000,
                numBytesRequested: 256/8
            ));
            return new HashSalt { Hash = encryptedPassword, Salt = salt };
        }

        public bool VerifyPassword(string enteredpassword, byte[] salt, string storedpassword)
        {
            string encryptedPassword = Convert.ToBase64String(KeyDerivation.Pbkdf2(
                password: enteredpassword,
                salt: salt,
                prf: KeyDerivationPrf.HMACSHA1,
                iterationCount: 10000,
                numBytesRequested: 256/8
            ));
            return encryptedPassword == storedpassword;
        }
    }
}
