using Microsoft.VisualStudio.TestTools.UnitTesting;
using API.Controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using APITests.Controllers;
using System.Net.Http;
using System.Net;
using Newtonsoft.Json;
using Microsoft.AspNetCore.Mvc.Testing;
using API.Models;
using API.ViewModels;

namespace API.Controllers.Tests
{
    [TestClass()]
    public class NovelControllerTests
    {
        public CustomWebApplicationFactory<Startup> _factory;
        public HttpClient _client;

        [TestInitialize]
        public async Task setup()
        {
            CustomWebApplicationFactory<Startup> factory = new CustomWebApplicationFactory<Startup>();
            _factory = factory;
            _client = factory.CreateClient(new WebApplicationFactoryClientOptions
            {
                AllowAutoRedirect = false
            });
            var request = new HttpRequestMessage(new HttpMethod("POST"), $"/novel/addfavorite?userid={1}&novelid={1}");
            var response = await _client.SendAsync(request);
            var request1 = new HttpRequestMessage(new HttpMethod("POST"), $"/novel/addfavorite?userid={1}&novelid={2}");
            var response1 = await _client.SendAsync(request1);
            var request2 = new HttpRequestMessage(new HttpMethod("POST"), $"/novel/addfavorite?userid={1}&novelid={3}");
            var response2 = await _client.SendAsync(request2);
        }
        [TestMethod()]
        public async Task GetNovelsTest()
        {
            //Arrange
            //Act
            var request = new HttpRequestMessage(new HttpMethod("GET"), "/novel/");
            var response = await _client.SendAsync(request);
            //Assert
            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
            var content = await response.Content.ReadAsStringAsync();

            List<Novel> n = JsonConvert.DeserializeObject<List<Novel>>(content);
            Assert.AreEqual("Novel1", n[0].Title);
            Assert.AreEqual("author1", n[0].Author);
            Assert.AreEqual("Novel2", n[1].Title);
            Assert.AreEqual("author2", n[1].Author);
            Assert.AreEqual("Novel3", n[2].Title);
            Assert.AreEqual("author1", n[2].Author);
        }
        [TestMethod()]
        public async Task GetNovel_ValidId_Succes()
        {
            //Arrange
            int id = 1;
            //Act
            var request = new HttpRequestMessage(new HttpMethod("GET"), $"/novel/{id}");
            var response = await _client.SendAsync(request);
            //Assert
            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
            var content = await response.Content.ReadAsStringAsync();

            Novel n = JsonConvert.DeserializeObject<Novel>(content);
            Assert.AreEqual("Novel1", n.Title);
            Assert.AreEqual("author1", n.Author);
        }
        [TestMethod()]
        public async Task GetNovel_InvalidId_Failure()
        {
            //Arrange
            int id = 123456;
            //Act
            var request = new HttpRequestMessage(new HttpMethod("GET"), $"/novel/{id}");
            var response = await _client.SendAsync(request);
            //Assert
            Assert.AreNotEqual(HttpStatusCode.OK, response.StatusCode);
        }
        [TestMethod()]
        public async Task InsertNovel_ValidInfo_Succes()
        {
            //Arrange
            string title = "newtitle";
            string author = "newauthor";
            string coverimage = "newcoverimage";
            string description = "newdescription";
            //Act
            var request = new HttpRequestMessage(new HttpMethod("POST"), $"/novel?title={title}&author={author}&coverimage={coverimage}&description={description}");
            var response = await _client.SendAsync(request);
            //Assert
            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
            var content = await response.Content.ReadAsStringAsync();

            Novel n = JsonConvert.DeserializeObject<Novel>(content);
            Assert.AreEqual(title, n.Title);
            Assert.AreEqual(author, n.Author);
            Assert.AreEqual(coverimage, n.CoverImage);
            Assert.AreEqual(description, n.Description);
        }
        [TestMethod()]
        public async Task DeleteNovel_ValidId_Succes()
        {
            //Arrange
            //Create a new novel to delete later
            string title = "newtitle";
            string author = "newauthor";
            string coverimage = "newcoverimage";
            string description = "newdescription";
            //Act
            var request = new HttpRequestMessage(new HttpMethod("POST"), $"/novel?title={title}&author={author}&coverimage={coverimage}&description={description}");
            var response = await _client.SendAsync(request);
            //Assert
            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
            var content = await response.Content.ReadAsStringAsync();

            Novel n = JsonConvert.DeserializeObject<Novel>(content);
            //Arrange
            int id = n.Id;
            //Act
            var request2 = new HttpRequestMessage(new HttpMethod("DELETE"), $"/novel/{id}");
            var response2 = await _client.SendAsync(request2);
            //Assert
            Assert.AreEqual(HttpStatusCode.OK, response2.StatusCode);
        }
        [TestMethod()]
        public async Task DeleteNovel_InvalidId_Failure()
        {
            //Arrange
            int id = 1234123412;
            //Act
            var request = new HttpRequestMessage(new HttpMethod("DELETE"), $"/novel/{id}");
            var response = await _client.SendAsync(request);
            //Assert
            Assert.AreNotEqual(HttpStatusCode.OK, response.StatusCode);
        }

        [TestMethod()]
        public async Task UpdateNovel_ValidInfo_Succes()
        {
            //Arrange
            int id = 1;
            string title = "newupdatedtitle";
            string author = "newupdatedauthor";
            string coverimage = "newupdatedcoverimage";
            string description = "newupdateddescription";
            //Act
            var request = new HttpRequestMessage(new HttpMethod("POST"), $"/novel?id={id}&title={title}&author={author}&coverimage={coverimage}&description={description}");
            var response = await _client.SendAsync(request);
            //Assert
            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
            var content = await response.Content.ReadAsStringAsync();

            Novel n = JsonConvert.DeserializeObject<Novel>(content);
            Assert.AreEqual(title, n.Title);
            Assert.AreEqual(author, n.Author);
            Assert.AreEqual(coverimage, n.CoverImage);
            Assert.AreEqual(description, n.Description);
        }
        [TestMethod()]
        public async Task UpdateNovel_InvalidInfo_Failure()
        {
            //Arrange
            int id = 12314;
            string title = "newupdatedtitle";
            string author = "newupdatedauthor";
            string coverimage = "newupdatedcoverimage";
            string description = "newupdateddescription";
            //Act
            var request = new HttpRequestMessage(new HttpMethod("PUT"), $"/novel/id={id}&title={title}&author={author}&coverimage={coverimage}&description={description}");
            var response = await _client.SendAsync(request);
            //Assert
            Assert.AreNotEqual(HttpStatusCode.OK, response.StatusCode);
        }
        [TestMethod()]
        public async Task GetFavoritesTest()
        {
            //Arrange
            int id = 1;
            //Act
            var request = new HttpRequestMessage(new HttpMethod("GET"), $"/novel/favorites?id={id}");
            var response = await _client.SendAsync(request);
            //Assert
            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
            var content = await response.Content.ReadAsStringAsync();

            List<Novel> n = JsonConvert.DeserializeObject<List<Novel>>(content);
            Assert.AreEqual("Novel1", n[0].Title);
            Assert.AreEqual("author1", n[0].Author);
            Assert.AreEqual("Novel2", n[1].Title);
            Assert.AreEqual("author2", n[1].Author);
            Assert.AreEqual("Novel3", n[2].Title);
            Assert.AreEqual("author1", n[2].Author);
        }
        [TestMethod()]
        public async Task AddFavorite_ValidInfo_Succes()
        {
            //Arrange
            int userid = 2;
            int novelid = 2;
            //Act
            var request = new HttpRequestMessage(new HttpMethod("POST"), $"/novel/addfavorite?userid={userid}&novelid={novelid}");
            var response = await _client.SendAsync(request);
            //Assert
            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);

            //Act
            var request2 = new HttpRequestMessage(new HttpMethod("GET"), $"/novel/favorites?id={userid}");
            var response2 = await _client.SendAsync(request2);
            //Assert
            Assert.AreEqual(HttpStatusCode.OK, response2.StatusCode);
            var content = await response2.Content.ReadAsStringAsync();

            List<Novel> n = JsonConvert.DeserializeObject<List<Novel>>(content);
            Assert.AreEqual("Novel2", n[0].Title);
            Assert.AreEqual("author2", n[0].Author);
        }
        [TestMethod()]
        public async Task AddFavorite_InvalidInfo_Failure()
        {
            //Arrange
            int userid = 2123;
            int novelid = 21234;
            //Act
            var request = new HttpRequestMessage(new HttpMethod("POST"), $"/novel/addfavorite?userid={userid}&novelid={novelid}");
            var response = await _client.SendAsync(request);
            //Assert
            Assert.AreNotEqual(HttpStatusCode.OK, response.StatusCode);
        }
        [TestMethod()]
        public async Task RemoveFavorite_ValidInfo_Succes()
        {
            //Arrange
            int userid = 1;
            int novelid = 2;
            //Act
            var request = new HttpRequestMessage(new HttpMethod("POST"), $"/novel/removefavorite?userid={userid}&novelid={novelid}");
            var response = await _client.SendAsync(request);
            //Assert
            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);

            //Act
            var request2 = new HttpRequestMessage(new HttpMethod("GET"), $"/novel/favorites?id={userid}");
            var response2 = await _client.SendAsync(request2);
            //Assert
            Assert.AreEqual(HttpStatusCode.OK, response2.StatusCode);
            var content = await response2.Content.ReadAsStringAsync();

            List<Novel> n = JsonConvert.DeserializeObject<List<Novel>>(content);
            Assert.AreEqual("Novel1", n[0].Title);
            Assert.AreEqual("author1", n[0].Author);
            Assert.AreEqual("Novel3", n[1].Title);
            Assert.AreEqual("author1", n[1].Author);
        }
        [TestMethod()]
        public async Task RemoveFavorite_InvalidInfo_Failure()
        {
            //Arrange
            int userid = 2123;
            int novelid = 21234;
            //Act
            var request = new HttpRequestMessage(new HttpMethod("POST"), $"/novel/addfavorite?userid={userid}&novelid={novelid}");
            var response = await _client.SendAsync(request);
            //Assert
            Assert.AreNotEqual(HttpStatusCode.OK, response.StatusCode);
        }
    }
}