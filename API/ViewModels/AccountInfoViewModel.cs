using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.ViewModels
{
    public class AccountInfoViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string RoleName { get; set; }
        public AccountInfoViewModel(int id, string name, string email, string rolename)
        {
            Id = id;
            Name = name;
            Email = email;
            RoleName = rolename;
        }
    }
}
