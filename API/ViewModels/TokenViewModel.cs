using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.ViewModels
{
    public class TokenViewModel
    {
        public string Token { get; set; }
        public TokenViewModel(string token)
        {
            Token = token;
        }
    }
}
