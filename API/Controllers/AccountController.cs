using API.Data;
using API.Models;
using API.Services;
using API.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    [Produces("application/json")]
    public class AccountController : ControllerBase
    {
        private readonly IAccountService _service;
        private readonly IJWTService _jwt;
        public AccountController(INovelContext context, IConfiguration config)
        {
            _service = new AccountService(context);
            _jwt = new JWTService(config);
        }
        [HttpPost("Register")]
        public TokenViewModel Register(string name, string email, string password)
        {
            bool registered = _service.RegisterAccount(name, email, password);
            if (registered)
            {
                Account a = _service.GetAccount(email);
                string token = _jwt.GenerateToken(a); //Generate token
                return new(token);
            }
            else
            {
                return null; //invalid request -> no account created
            }
        }
        [HttpPost("Login")]
        public TokenViewModel Login(string email, string password)
        {
            if(!_service.LoginAccount(email, password))
            {
                throw new InvalidOperationException("Invalid info");
            }
            Account a = _service.GetAccount(email);
            string token = _jwt.GenerateToken(a); //Generate token
            return new(token);
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
            return avms.ToArray();
        }
        [HttpGet("{id}")]
        public AccountViewModel Get(int id)
        {
            Account a = _service.GetAccount(id);
            return new(a);
        }
        [HttpGet("Info")]
        public AccountInfoViewModel Info(string token)
        {
            if (!_jwt.ValidateToken(token))
            {
                throw new InvalidOperationException("Invalid token");
            }
            string email = _jwt.GetClaim(token, "Email");
            Account a = _service.GetAccount(email);
            string name = a.Name;
            string role = a.Role.Name;
            int id = a.Id;
            return new(id, name, email, role);
        }
        [HttpGet("Roles")]
        public IEnumerable<RoleViewModel> Roles()
        {
            List<Role> roles = _service.GetRoles().ToList();
            List<RoleViewModel> rvms = new();
            for (int i = 0; i < roles.Count(); i++)
            {
                rvms.Add(new(roles[i]));
            }
            return rvms.ToArray();
        }
        [HttpPost("SetRole")]
        public bool SetRole(string token, int userid, int roleid)
        {
            if (!_jwt.ValidateToken(token))
            {
                throw new InvalidOperationException("Invalid token");
            }
            if ("Admin" !=_jwt.GetClaim(token,"Role"))
            {
                throw new UnauthorizedAccessException("No permission");
            }
            return _service.SetRole(userid, roleid);
        }
    }
}
