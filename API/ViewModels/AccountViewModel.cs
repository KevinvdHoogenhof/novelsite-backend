using API.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.ViewModels
{
    public class AccountViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string Token { get; set; }
        public RoleViewModel Role { get; set; }
        public List<NovelViewModel> Favorites { get; set; }
        public AccountViewModel(Account account)
        {
            Id = account.Id;
            Name = account.Name;
            Email = account.Email;
            Password = account.Password;
            if (account.Role != null)
            {
                Role = new(account.Role);
            }
            Favorites = new();
            if (account.Favorites != null)
            {
                for (int i = 0; i < account.Favorites.Count; i++)
                {
                    Favorites.Add(new(account.Favorites[i].Novel));
                }
            }
        }
        public AccountViewModel(Account account, string token)
        {
            Id = account.Id;
            Name = account.Name;
            Email = account.Email;
            Password = account.Password;
            if (account.Role != null)
            {
                Role = new(account.Role);
            }
            Favorites = new();
            if (account.Favorites != null)
            {
                for (int i = 0; i < account.Favorites.Count; i++)
                {
                    Favorites.Add(new(account.Favorites[i].Novel));
                }
            }
            Token = token;
        }
    }
}
