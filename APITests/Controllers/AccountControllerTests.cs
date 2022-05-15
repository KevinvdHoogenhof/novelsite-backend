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
            var request = new HttpRequestMessage(new HttpMethod("GET"), "/account/");
            var response = await _client.SendAsync(request);

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
            string email = "email1";
            string pw = "pw1";
            var request = new HttpRequestMessage(new HttpMethod("POST"), $"/account/Login?email={email}&password={pw}");
            var response = await _client.SendAsync(request);

            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
            var content = await response.Content.ReadAsStringAsync();

            TokenViewModel token = JsonConvert.DeserializeObject<TokenViewModel>(content);
            Assert.IsInstanceOfType(token.Token, typeof(string));
        }
        [TestMethod()]
        public async Task LoginAccount_InvalidInfo_Failure()
        {
            string email = "emailnotcorrect";
            string pw = "pw1";
            var request = new HttpRequestMessage(new HttpMethod("POST"), $"/account/Login?email={email}&password={pw}");
            var response = await _client.SendAsync(request);

            Assert.AreNotEqual(HttpStatusCode.OK, response.StatusCode);
        }
    }
}