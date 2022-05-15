using API.Models;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace API.Services
{
    public class JWTService : IJWTService
    {
        private IConfiguration _config;
        public static readonly string ClaimsName = "Name";
        public static readonly string ClaimsEmail = "Email";
        public static readonly string ClaimsRole = "Role";
        public JWTService(IConfiguration config)
        {
            _config = config;
        }
        public string GenerateToken(Account account)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(_config["Jwt:Key"]));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var claims = new[]
            {
                new Claim(ClaimsName, account.Name),
                new Claim(ClaimsEmail, account.Email),
                new Claim(ClaimsRole, account.Role.Name)
            };

            var tokenHandler = new JwtSecurityTokenHandler();
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.Now.AddDays(7),
                Issuer = _config["Jwt:Issuer"],
                Audience = _config["Jwt:Audience"],
                SigningCredentials = credentials
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);

            //var token = new JwtSecurityToken(_config["Jwt:Issuer"],
            //    _config["Jwt:Audience"],
            //    claims,
            //    expires: DateTime.Now.AddDays(7),
            //    signingCredentials: credentials);

            return tokenHandler.WriteToken(token);
        }

        public bool ValidateToken(string token)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(_config["Jwt:Key"]));
            var tokenHandler = new JwtSecurityTokenHandler();

            try
            {
                tokenHandler.ValidateToken(token, new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidIssuer = _config["Jwt:Issuer"],
                    ValidAudience = _config["Jwt:Audience"],
                    IssuerSigningKey = securityKey
                }, out SecurityToken validatedToken); ;
            }
            catch
            {

                return false;
            }
            return true;
        }

        public string GetClaim(string token, string claimType)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var securityToken = tokenHandler.ReadToken(token) as JwtSecurityToken;
            var stringClaimValue = securityToken.Claims.First(claim => claim.Type == claimType).Value;
            return stringClaimValue;
        }
    }
}
