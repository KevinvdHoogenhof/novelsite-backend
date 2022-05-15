using Microsoft.VisualStudio.TestTools.UnitTesting;
using API.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using API.Models;

namespace API.Services.Tests
{
    [TestClass()]
    public class JWTServiceTests
    {
        public IJWTService _jwt;
        [TestInitialize]
        public void Setup()
        {
            var myConfiguration = new Dictionary<string, string>
            {
                {"Jwt:Key", "securekey_ashduihwiuyegr1234"},
                {"Jwt:Issuer", "https://localhost:9001/"},
                {"Jwt:Audience", "https://localhost:9001/"}
            };

            var config = new ConfigurationBuilder()
                .AddInMemoryCollection(myConfiguration)
                .Build();
            _jwt = new JWTService(config);
        }
        [TestMethod()]
        public void GenerateTokenTest()
        {
            //Arrange
            Role r = new() { Name = "Standard" };
            Account a = new() { Name = "name1", Email = "email1", Role = r };
            //Assert
            Assert.IsInstanceOfType(_jwt.GenerateToken(a),typeof(string));
        }
        [TestMethod()]
        public void ValidateToken_ValidToken_True()
        {
            //Arrange
            Role r = new() { Name = "Standard" };
            Account a = new() { Name = "name1", Email = "email1", Role = r };
            //Act
            string token = _jwt.GenerateToken(a);
            //Assert
            Assert.IsTrue(_jwt.ValidateToken(token));
        }
        [TestMethod()]
        public void ValidateToken_InvalidToken_False()
        {
            //Act
            string token = "InvalidToken";
            //Assert
            Assert.IsFalse(_jwt.ValidateToken(token));
        }
        [TestMethod()]
        public void GetClaimEmailFromToken()
        {
            //Arrange
            string actualemail = "emailtogetfromclaim";
            Role r = new() { Name = "Standard" };
            Account a = new() { Name = "name1", Email = actualemail, Role = r };
            //Act
            string token = _jwt.GenerateToken(a);
            string claim = "Email";
            Assert.AreEqual(actualemail,_jwt.GetClaim(token,claim));
        }
    }
}