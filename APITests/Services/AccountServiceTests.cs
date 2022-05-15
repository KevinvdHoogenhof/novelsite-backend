using Microsoft.VisualStudio.TestTools.UnitTesting;
using API.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using API.Data;
using Microsoft.EntityFrameworkCore;
using API.Models;

namespace API.Services.Tests
{
    [TestClass()]
    public class AccountServiceTests
    {
        public IAccountService _service;
        public IEncryptionService _encrypt;
        [TestInitialize]
        public void Setup()
        {
            _encrypt = new EncryptionService();
            var options = new DbContextOptionsBuilder<NovelContext>()
                .UseInMemoryDatabase(databaseName: "TestNovelDB")
                .Options;

            INovelContext _context = new NovelContext(options);

            ////
            if (!_context.Roles.Any())
            {
                _context.Roles.Add(new Role() { Name = "Standard" });
                _context.SaveChanges();
                _context.Roles.Add(new Role() { Name = "Admin" });
                _context.SaveChanges();
            }

            ////
            if (!_context.Accounts.Any())
            {
                ////
                var hashsalt = _encrypt.EncryptPassword("pw1");
                _context.Accounts.Add(new Account() { Name = "name1", Email = "email1", Password = hashsalt.Hash, StoredSalt = hashsalt.Salt, Role = _context.Roles.Find(1) });
                _context.SaveChanges();

                ////
                var hashsalt2 = _encrypt.EncryptPassword("admin");
                _context.Accounts.Add(new Account() { Name = "admin", Email = "admin", Password = hashsalt2.Hash, StoredSalt = hashsalt2.Salt, Role = _context.Roles.Find(2) });
                _context.SaveChanges();

                ////
                var hashsalt3 = _encrypt.EncryptPassword("pw3");
                _context.Accounts.Add(new Account() { Name = "name3", Email = "email3", Password = hashsalt3.Hash, StoredSalt = hashsalt3.Salt, Role = _context.Roles.Find(1) });
                _context.SaveChanges();
            }
            _service = new AccountService(_context);
        }
        [TestMethod()]
        public void GetAllAccountsTest()
        {
            //Assert
            Assert.AreEqual(3, _service.GetAccounts().Count());
        }
        [TestMethod()]
        public void RegisterAccount_EmailHasNotBeenUsed_True()
        {
            //Arrange
            string name = "name";
            string email = "emailthathasnotbeenusedbefore";
            string password = "verysecurepassword";
            //Act
            //Assert
            Assert.IsTrue(_service.RegisterAccount(name, email, password));
        }
        [TestMethod()]
        public void RegisterAccount_EmailHasBeenUsed_False()
        {
            //Arrange
            string name = "name";
            string email = "email3"; //Email was used before
            string password = "verysecurepassword";
            //Act
            //Assert
            Assert.IsFalse(_service.RegisterAccount(name, email, password));
        }
        [TestMethod()]
        public void LoginAccount_ValidCredentials_True()
        {
            //Arrange
            string email = "email1";
            string password = "pw1";
            //Act
            //Assert
            Assert.IsTrue(_service.LoginAccount(email, password));
        }
        [TestMethod()]
        public void LoginAccount_InvalidCredentials_False()
        {
            //Arrange
            string email = "email1";
            string password = "IncorrectPW";
            //Act
            //Assert
            Assert.IsFalse(_service.LoginAccount(email, password));
        }
        [TestMethod()]
        public void GetAccount_WithId()
        {
            //Assert
            //Try novel id 1;
            Account a = _service.GetAccount(1);
            //Check if fields are valid
            Assert.AreEqual("name1", a.Name);
            Assert.AreEqual("email1", a.Email);
        }
        [TestMethod()]
        public void GetAccount_WithInvalidId_Exception()
        {
            //Assert
            Assert.ThrowsException<InvalidOperationException>(() => _service.GetAccount(123));
        }
        [TestMethod()]
        public void GetAccount_WithEmail()
        {
            //Assert
            //Try novel id 1;
            Account a = _service.GetAccount("email1");
            //Check if fields are valid
            Assert.AreEqual("name1", a.Name);
            Assert.AreEqual("email1", a.Email);
        }
        [TestMethod()]
        public void GetAccount_WithInvalidEmail_Exception()
        {
            //Assert
            Assert.ThrowsException<InvalidOperationException>(() => _service.GetAccount("invalidemailthathasnotbeenusedbefore"));
        }
        [TestMethod()]
        public void GetAllRolesTest()
        {
            //Assert
            Assert.AreEqual(2, _service.GetRoles().Count());
        }
        [TestMethod()]
        public void SetRoleTest()
        {
            //Act
            //Try novel id 1;
            Account a = _service.GetAccount(1);
            //Check current role is Standard
            Assert.AreEqual("Standard", a.Role.Name);
            //Change role
            _service.SetRole(1,2);
            Account acc = _service.GetAccount(1);
            //Check if role was set to admin
            Assert.AreEqual("Admin", acc.Role.Name);
        }
    }
}