using Microsoft.VisualStudio.TestTools.UnitTesting;
using API.Controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Net.Http;
using API.Services;
using API.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Xunit;
using Microsoft.AspNetCore.Mvc.Testing;
using APITests.Controllers;
using API.Models;
using Newtonsoft.Json;
using API.ViewModels;

namespace API.Controllers.Tests
{
    [TestClass()]
    public class AccountControllerTests : IClassFixture<CustomWebApplicationFactory<Startup>>
    {
        public CustomWebApplicationFactory<Startup> _factory;
        public HttpClient _client;

        [TestInitialize]
        public void setup()
        {
            CustomWebApplicationFactory<Startup> factory = new CustomWebApplicationFactory<Startup>();
            _factory = factory;
            _client = factory.CreateClient(new WebApplicationFactoryClientOptions
            {
                AllowAutoRedirect = false
            });
        }
        [TestMethod()]
        public async Task GetAccounts()
        {
            //Arrange
            //Act
            var request = new HttpRequestMessage(new HttpMethod("GET"), "/account/");
            var response = await _client.SendAsync(request);
            //Assert
            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
            var content = await response.Content.ReadAsStringAsync();

            List<Account> a = JsonConvert.DeserializeObject<List<Account>>(content);
            Assert.AreEqual("name1", a[0].Name);
            Assert.AreEqual("email1", a[0].Email);
            Assert.AreEqual("admin", a[1].Name);
            Assert.AreEqual("admin", a[1].Email);
            Assert.AreEqual("name3", a[2].Name);
            Assert.AreEqual("email3", a[2].Email);
        }
        [TestMethod()]
        public async Task LoginAccount_ValidInfo_Succes()
        {
            //Arrange
            string email = "email1";
            string pw = "pw1";
            //Act
            var request = new HttpRequestMessage(new HttpMethod("POST"), $"/account/login?email={email}&password={pw}");
            var response = await _client.SendAsync(request);
            //Assert
            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
            var content = await response.Content.ReadAsStringAsync();

            TokenViewModel token = JsonConvert.DeserializeObject<TokenViewModel>(content);
            Assert.IsInstanceOfType(token.Token, typeof(string));
        }
        [TestMethod()]
        public async Task LoginAccount_InvalidInfo_Failure()
        {
            //Arrange
            string email = "emailnotcorrect";
            string pw = "pw1";
            //Act
            var request = new HttpRequestMessage(new HttpMethod("POST"), $"/account/login?email={email}&password={pw}");
            var response = await _client.SendAsync(request);
            //Assert
            Assert.AreNotEqual(HttpStatusCode.OK, response.StatusCode);
        }
        [TestMethod()]
        public async Task RegisterAccount_ValidInfo_Succes()
        {
            //Arrange
            string name = "newname1";
            string email = "unusedemail";
            string pw = "pw1";
            //Act
            var request = new HttpRequestMessage(new HttpMethod("POST"), $"/account/register?name={name}&email={email}&password={pw}");
            var response = await _client.SendAsync(request);
            //Asert
            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
            var content = await response.Content.ReadAsStringAsync();

            TokenViewModel token = JsonConvert.DeserializeObject<TokenViewModel>(content);
            Assert.IsInstanceOfType(token.Token, typeof(string));
        }
        [TestMethod()]
        public async Task RegisterAccount_InvalidInfo_Failure()
        {
            //Arrange
            string name = "newname1";
            string email = "email1";
            string pw = "pw1";
            //Act
            var request = new HttpRequestMessage(new HttpMethod("POST"), $"/account/register?name={name}&email={email}&password={pw}");
            var response = await _client.SendAsync(request);
            //Assert
            Assert.AreNotEqual(HttpStatusCode.OK, response.StatusCode);
        }
        [TestMethod()]
        public async Task GetAccountInfo_Validtoken_Info()
        {
            //Arrange
            string email = "email1";
            string pw = "pw1";
            //Act
            var request = new HttpRequestMessage(new HttpMethod("POST"), $"/account/login?email={email}&password={pw}"); //Login to account to get valid token
            var response = await _client.SendAsync(request);
            //Assert
            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
            var content = await response.Content.ReadAsStringAsync();

            TokenViewModel token = JsonConvert.DeserializeObject<TokenViewModel>(content);
            Assert.IsInstanceOfType(token.Token, typeof(string));

            //Arrange
            var newrequest = new HttpRequestMessage(new HttpMethod("GET"), $"/account/info?token={token.Token}");
            var newresponse = await _client.SendAsync(newrequest);
            //Act
            Assert.AreEqual(HttpStatusCode.OK, newresponse.StatusCode);
            var newcontent = await newresponse.Content.ReadAsStringAsync();

            //Assert
            AccountInfoViewModel a = JsonConvert.DeserializeObject<AccountInfoViewModel>(newcontent);
            Assert.AreEqual(1, a.Id);
            Assert.AreEqual("name1", a.Name);
            Assert.AreEqual("email1", a.Email);
            Assert.AreEqual("Standard", a.RoleName);
        }
        [TestMethod()]
        public async Task GetAccountInfo_Invalidtoken_Failure()
        {
            //Arrange
            string token = "faketoken";
            //Act
            var request = new HttpRequestMessage(new HttpMethod("GET"), $"/account/info?token={token}");
            var response = await _client.SendAsync(request);
            //Assert
            Assert.AreNotEqual(HttpStatusCode.OK, response.StatusCode);
        }
        [TestMethod()]
        public async Task GetRoles()
        {
            //Arrange
            //Act
            var request = new HttpRequestMessage(new HttpMethod("GET"), "/account/roles/");
            var response = await _client.SendAsync(request);
            //Assert
            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
            var content = await response.Content.ReadAsStringAsync();

            List<Role> r = JsonConvert.DeserializeObject<List<Role>>(content);
            Assert.AreEqual("Standard", r[0].Name);
            Assert.AreEqual("Admin", r[1].Name);
        }
    }
}