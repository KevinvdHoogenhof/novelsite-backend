using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.Services
{
    public interface IEncryptionService
    {
        public HashSalt EncryptPassword(string password);
        public bool VerifyPassword(string enteredpassword, byte[] salt, string storedpassword);
    }
}
