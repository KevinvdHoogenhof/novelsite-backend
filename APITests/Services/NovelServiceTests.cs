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
    public class NovelServiceTests
    {
        public INovelService _service;
        [TestInitialize]
        public void Setup()
        {
            var options = new DbContextOptionsBuilder<NovelContext>()
                .UseInMemoryDatabase(databaseName: "TestNovelDB")
                .Options;

            INovelContext _context = new NovelContext(options);
            ////
            if (!_context.Novels.Any())
            {
                _context.Novels.Add(new Novel() { Title = "Novel1", Author = "author1", CoverImage = "coverimage1", Description = "novel 1 description" });
                _context.SaveChanges();
                _context.Novels.Add(new Novel() { Title = "Novel2", Author = "author2", CoverImage = "coverimage2", Description = "description of novel 2" });
                _context.SaveChanges();
                _context.Novels.Add(new Novel() { Title = "Novel3", Author = "author1", CoverImage = "coverimage3", Description = "second novel of author1" });
                _context.SaveChanges();
                _context.Novels.Add(new Novel() { Title = "Novel4", Author = "author3", CoverImage = "coverimage4", Description = "description of novel #4" });
                _context.SaveChanges();
            }
            ////
            if (!_context.Genres.Any())
            {
                _context.Genres.Add(new Genre() { Name = "Action" });
                _context.SaveChanges();
                _context.Genres.Add(new Genre() { Name = "Adventure" });
                _context.SaveChanges();
                _context.Genres.Add(new Genre() { Name = "Comedy" });
                _context.SaveChanges();
                _context.Genres.Add(new Genre() { Name = "Drama" });
                _context.SaveChanges();
                _context.Genres.Add(new Genre() { Name = "Fantasy" });
                _context.SaveChanges();
                _context.Genres.Add(new Genre() { Name = "Historical" });
                _context.SaveChanges();
                _context.Genres.Add(new Genre() { Name = "Romance" });
                _context.SaveChanges();
            }
            ///
            if (!_context.NovelGenres.Any())
            {
                // Novel 1
                _context.NovelGenres.Add(new() { Novel = _context.Novels.Find(1), Genre = _context.Genres.Find(1) });
                _context.SaveChanges();
                _context.NovelGenres.Add(new() { Novel = _context.Novels.Find(1), Genre = _context.Genres.Find(2) });
                _context.SaveChanges();
                // Novel 2
                _context.NovelGenres.Add(new() { Novel = _context.Novels.Find(2), Genre = _context.Genres.Find(2) });
                _context.SaveChanges();
                // Novel 3
                _context.NovelGenres.Add(new() { Novel = _context.Novels.Find(3), Genre = _context.Genres.Find(2) });
                _context.SaveChanges();
                _context.NovelGenres.Add(new() { Novel = _context.Novels.Find(3), Genre = _context.Genres.Find(3) });
                _context.SaveChanges();
                _context.NovelGenres.Add(new() { Novel = _context.Novels.Find(3), Genre = _context.Genres.Find(6) });
                _context.SaveChanges();
                // Novel 4
                _context.NovelGenres.Add(new() { Novel = _context.Novels.Find(4), Genre = _context.Genres.Find(4) });
                _context.SaveChanges();
                _context.NovelGenres.Add(new() { Novel = _context.Novels.Find(4), Genre = _context.Genres.Find(5) });
                _context.SaveChanges();
                _context.NovelGenres.Add(new() { Novel = _context.Novels.Find(4), Genre = _context.Genres.Find(7) });
                _context.SaveChanges();
            }

            if (!_context.Favorites.Any())
            {
                // Account 1
                _context.Favorites.Add(new() { Novel = _context.Novels.Find(1), Account = _context.Accounts.Find(1) });
                _context.SaveChanges();
                _context.Favorites.Add(new() { Novel = _context.Novels.Find(2), Account = _context.Accounts.Find(1) });
                _context.SaveChanges();
                _context.Favorites.Add(new() { Novel = _context.Novels.Find(3), Account = _context.Accounts.Find(1) });
                _context.SaveChanges();
            }

            _service = new NovelService(_context);
        }

        [TestMethod()]
        public void GetAllNovelsTest()
        {
            //Assert
            Assert.AreEqual(4, _service.GetNovels().Count());
        }
        [TestMethod()]
        public void GetNovelTest()
        {
            //Assert
            //Try novel id 1;
            Novel novel = _service.GetNovel(1);
            //Check if fields are valid
            Assert.AreEqual("Novel1", novel.Title);
            Assert.AreEqual("author1", novel.Author);
            Assert.AreEqual("coverimage1", novel.CoverImage);
            Assert.AreEqual("Action", novel.Genres[0].Genre.Name);
        }
        [TestMethod()]
        public void InsertNovel_Inserted()
        {
            //Arrange
            Novel novel = new() { Title = "testtitle7", Author = "testauthor7", CoverImage = "testcoverimage7", Description = "testdescription7" };
            //Act
            novel.Id = _service.InsertNovel(novel);
            Novel n = _service.GetNovel(novel.Id);
            //Assert
            Assert.AreEqual(novel.Title, n.Title);
        }
        [TestMethod()]
        public void UpdateNovel_Updated()
        {
            //Arrange
            //Updated version of novel with id = 1
            Novel noveltoupdate = _service.GetNovel(1);
            string updatedtitle = "NewTitle";
            noveltoupdate.Title = updatedtitle;
            //Act
            //Assert
            Assert.IsTrue(_service.UpdateNovel(noveltoupdate));
            Novel novel = _service.GetNovel(1); 
            //
            Assert.AreEqual(updatedtitle, novel.Title);
        }
        [TestMethod()]
        public void DeleteNovel_Deleted()
        {
            //Arrange
            Novel novel = new() { Title = "testtitlefornoveltodelete", Author = "testauthorfornoveltodelete", CoverImage = "testcoverimagefornoveltodelete", Description = "testdescriptionfornoveltodelete" };
            //Act
            novel.Id = _service.InsertNovel(novel);
            //delete novel with that we just inserted
            Assert.IsTrue(_service.DeleteNovel(novel.Id));
            //
            //Assert
            Assert.ThrowsException<InvalidOperationException>(() => _service.GetNovel(novel.Id));
        }
        [TestMethod()]
        public void GetFavoritesTest()
        {
            //Assert
            Assert.AreEqual(4, _service.GetFavorites(1).Count());
        }
        [TestMethod()]
        public void AddFavorite_ValidInfo_True()
        {
            //Arrange
            int userid = 1;
            int novelid = 4;
            //Act
            bool result = _service.AddFavorite(userid,novelid);
            //Assert
            Assert.IsTrue(result);
            Assert.AreEqual(4, _service.GetFavorites(1).Count());
        }
        [TestMethod()]
        public void AddFavorite_InvalidInfo_False()
        {
            //Arrange
            int userid = 7;
            int novelid = 6;
            //Assert
            Assert.ThrowsException<InvalidOperationException>(() => _service.AddFavorite(userid, novelid));
        }
        [TestMethod()]
        public void RemoveFavorite_ValidInfo_True()
        {
            //Arrange
            int userid = 1;
            int novelid = 4;
            //Act
            bool result = _service.RemoveFavorite(userid, novelid);
            //Assert
            Assert.IsTrue(result);
            Assert.AreEqual(3, _service.GetFavorites(1).Count());
        }
        [TestMethod()]
        public void RemoveFavorite_InvalidInfo_False()
        {
            //Arrange
            int userid = 7;
            int novelid = 6;
            //Assert
            Assert.ThrowsException<InvalidOperationException>(() => _service.RemoveFavorite(userid, novelid));
        }
    }
}