using API.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.Services
{
    public interface IAccountService
    {
        public bool RegisterAccount(string name, string email, string password);
        public bool LoginAccount(string email, string password);
        public IEnumerable<Account> GetAccounts();
        public Account GetAccount(int id);
        public Account GetAccount(string email);
        public IEnumerable<Role> GetRoles();
        public bool SetRole(int userid, int roleid);
    }
}
