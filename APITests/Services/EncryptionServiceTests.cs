using Microsoft.VisualStudio.TestTools.UnitTesting;
using API.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace API.Services.Tests
{
    [TestClass()]
    public class EncryptionServiceTests
    {
        public IEncryptionService _encrypt;
        [TestInitialize]
        public void Setup()
        {
            _encrypt = new EncryptionService();
        }
        [TestMethod()]
        public void SamePasswordHasDifferentHash()
        {
            //Arrange
            string pw = "pwtest1";
            //Act
            HashSalt hs = _encrypt.EncryptPassword(pw);
            HashSalt hs2 = _encrypt.EncryptPassword(pw);
            //Assert
            Assert.AreNotEqual(hs.Hash,hs2.Hash);
        }
        [TestMethod()]
        public void CorrectPassword_VerifyPassword_True()
        {
            //Arrange
            string pw = "pwtest1";
            //Act
            HashSalt hs = _encrypt.EncryptPassword(pw);
            //Assert
            Assert.IsTrue(_encrypt.VerifyPassword(pw,hs.Salt,hs.Hash));
        }
        [TestMethod()]
        public void IncorrectPassword_VerifyPassword_False()
        {
            //Arrange
            string pw = "pwtest1";
            string incorrectpw = "pwtest11";
            //Act
            HashSalt hs = _encrypt.EncryptPassword(pw);
            //Assert
            Assert.IsFalse(_encrypt.VerifyPassword(incorrectpw, hs.Salt, hs.Hash));
        }
    }
}