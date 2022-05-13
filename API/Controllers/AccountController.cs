using API.Data;
using API.Models;
using API.Services;
using API.ViewModels;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class AccountController : ControllerBase
    {
        private readonly IAccountService _service;
        public AccountController(INovelContext context)
        {
            _service = new AccountService(context);
        }
        [HttpPost("Register")]
        public string Register(string name, string email, string password)
        {
            Account a = new() { Name = name, Email = email, Password = password };
            bool registered = _service.RegisterAccount(a);
            if (registered)
            {
                string token = "token"; //Generate token
                return token;
            }
            else
            {
                return null; //invalid request -> no account created
            }
        }
        [HttpPost("Login")]
        public string Login(string email, string password)
        {
            Account a = _service.LoginAccount(email, password);
            string token = "token"; //Generate token
            return token;
        }
        [HttpGet]
        public IEnumerable<AccountViewModel> Get()
        {
            List<Account> accounts = _service.GetAccounts().ToList();
            List<AccountViewModel> avms = new();
            for (int i = 0; i < accounts.Count(); i++)
            {
                avms.Add(new(accounts[i]));
            }
            return avms;
        }
        [HttpGet("{id}")]
        public AccountViewModel Get(int id)
        {
            Account a = _service.GetAccount(id);
            return new(a);
        }
    }
}
