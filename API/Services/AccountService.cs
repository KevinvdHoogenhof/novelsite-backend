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
        public AccountService(INovelContext context)
        {
            _context = context;
        }
        public bool InsertAccount(Account account)
        {
            _context.Accounts.Add(account);
            _context.SaveChanges();
            return true;
        }
        //Check if exists
        private bool DoesEmailExist(string email)
        {
            return _context.Accounts.Any(a => a.Email == email);
        }

        //Register
        public bool RegisterAccount(Account account)
        {
            if (!DoesEmailExist(account.Email))
            {
                //Check if default role exists
                if (_context.Roles.Find(1) == null)
                {
                    _context.Roles.Add(new() { Id = 1, Name = "Standard"});
                    _context.SaveChanges();
                }
                account.Role = _context.Roles.Find(1); //set standard role
                return InsertAccount(account);
            }
            else
            {
                return false;
            }
        }
        public Account LoginAccount(string email, string password)
        {
            Account acc = _context.Accounts.Where(a => a.Email == email)
                .Include(a => a.Role)
                .Include(a => a.Favorites)
                .ThenInclude(a => a.Novel)
                .Include(a => a.Comments)
                .Single();
            if (acc.Password == password) //Check password
            {
                return acc;
            }
            else
            {
                return null;
            }
        }
    }
}
