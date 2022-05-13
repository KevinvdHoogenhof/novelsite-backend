using API.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.Services
{
    public interface IAccountService
    {
        public bool RegisterAccount(Account account);
        public Account LoginAccount(string email, string password);
        public IEnumerable<Account> GetAccounts();
        public Account GetAccount(int id);
    }
}
