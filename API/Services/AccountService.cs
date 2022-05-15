using API.Data;
using API.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.Services
{
    public class AccountService : IAccountService
    {
        private readonly INovelContext _context;
        private readonly IEncryptionService _encrypt;
        public AccountService(INovelContext context)
        {
            _context = context;
            _encrypt = new EncryptionService();
        }
        public bool InsertAccount(Account account)
        {
            try
            {
                _context.Accounts.Add(account);
                _context.SaveChanges();
            }
            catch
            {
                return false;
            }
            return true;
        }
        //Check if exists
        private bool DoesEmailExist(string email)
        {
            return _context.Accounts.Any(a => a.Email == email);
        }

        //Register
        public bool RegisterAccount(string name, string email, string password)
        {
            if (!DoesEmailExist(email))
            {
                //Encypt password
                Account a = new();
                a.Name = name;
                a.Email = email;
                var hashsalt = _encrypt.EncryptPassword(password);
                a.Password = hashsalt.Hash;
                a.StoredSalt = hashsalt.Salt;

                a.Role = _context.Roles.Find(1); //set standard role
                return InsertAccount(a);
            }
            else
            {
                return false;
            }
        }
        public bool LoginAccount(string email, string password)
        {
            var account = _context.Accounts.FirstOrDefault(a => a.Email == email);
            var doesPasswordMatch = _encrypt.VerifyPassword(password, account.StoredSalt, account.Password);
            return doesPasswordMatch;
        }

        public IEnumerable<Account> GetAccounts()
        {
            try
            {
                return _context.Accounts
                    .Include(a => a.Role)
                    .Include(a => a.Favorites)
                    .ThenInclude(a => a.Novel)
                    .Include(a => a.Comments)
                    .ToArray();
            }
            catch (InvalidOperationException)
            {
                throw new InvalidOperationException("Could not get accounts");
            }
        }

        public Account GetAccount(int id)
        {
            try
            {
                Account acc = _context.Accounts.Where(a => a.Id == id)
                    .Include(a => a.Role)
                    .Include(a => a.Favorites)
                    .ThenInclude(a => a.Novel)
                    .Include(a => a.Comments)
                    .Single();
                return acc;
            }
            catch (InvalidOperationException)
            {
                throw new InvalidOperationException("Could not find account");
            }
        }
        public Account GetAccount(string email)
        {
            try
            {
                Account acc = _context.Accounts.Where(a => a.Email == email)
                    .Include(a => a.Role)
                    .Include(a => a.Favorites)
                    .ThenInclude(a => a.Novel)
                    .Include(a => a.Comments)
                    .Single();
                return acc;
            }
            catch (InvalidOperationException)
            {
                throw new InvalidOperationException("Could not find account");
            }
        }
        public IEnumerable<Role> GetRoles()
        {
            try
            {
                return _context.Roles
                    .ToArray();
            }
            catch (InvalidOperationException)
            {
                throw new InvalidOperationException("Could not get roles");
            }
        }

        public bool SetRole(int userid, int roleid)
        {
            try
            {

                Account acc = _context.Accounts.Where(a => a.Id == userid)
                    .Include(a => a.Role)
                    .Single();
                acc.Role = _context.Roles.Find(roleid);
                _context.SaveChanges();
                return true;
            }
            catch
            {

                return false;
            }
        }
    }
}
