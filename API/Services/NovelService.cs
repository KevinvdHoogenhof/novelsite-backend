using API.Data;
using API.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.Services
{
    public class NovelService : INovelService
    {
        private readonly INovelContext _context;
        //public NovelService(INovelContext)
        //{
        //    _context = new NovelContext();
        //    /*
        //    var options = new DbContextOptionsBuilder<NovelContext>()
        //                    .UseInMemoryDatabase(databaseName: "TestNovelDB")
        //                    .Options;
        //    _context = new NovelContext(options);
        //    var testnovel = _context.Novels.FirstOrDefault();
        //    if (testnovel == null)
        //    {
        //        Novel novel = new() { Author = "test1", Title = "test1", CoverImage = "test1", Description = "test1" };
        //        Novel novel1 = new() { Author = "test2", Title = "test2", CoverImage = "test2", Description = "test2" };
        //        _context.Novels.Add(novel);
        //        _context.Novels.Add(novel1);
        //    }
        //    _context.SaveChanges();
        //    */
        //}
        public NovelService(INovelContext context)
        {
            _context = context;
        }
        public IEnumerable<Novel> GetNovels()
        {
            return _context.Novels
                .Include(n => n.Genres)
                .ThenInclude(n => n.Genre)
                .ToArray();
        }
        public Novel GetNovel(int id)
        {
            try
            {
                return _context.Novels
                .Where(n => n.Id == id)
                .Include(n => n.Genres)
                .ThenInclude(n => n.Genre)
                .Include(n => n.Comments)
                .Single();
            }
            catch
            {

                throw new InvalidOperationException($"Could not find novel with id {id}");
            }
        }
        public int InsertNovel(Novel novel)
        {
            _context.Novels.Add(novel);
            _context.SaveChanges();

            return novel.Id;
        }
        public bool DeleteNovel(int id)
        {
            try
            {
                Novel novel = _context.Novels.Find(id);
                _context.Novels.Remove(novel);
                _context.SaveChanges();
                return true;
            }
            catch (InvalidOperationException)
            {
                throw new InvalidOperationException("Error deleting novel");
            }
        }
        public bool UpdateNovel(Novel novel)
        {
            try
            {
                Novel n = _context.Novels.Find(novel.Id);
                n.Title = novel.Title;
                n.Author = novel.Author;
                n.CoverImage = novel.CoverImage;
                n.Description = novel.Description;
                _context.SaveChanges();
                return true;
            }
            catch (InvalidOperationException)
            {
                throw new InvalidOperationException("Error updating novel");
            }
        }
        public IEnumerable<Novel> GetFavorites(int id)
        {
            Account a = _context.Accounts.Where(a => a.Id == id)
                    .Include(a => a.Favorites)
                    .ThenInclude(a => a.Novel)
                    .Single();
            List<Novel> novels = new();
            for (int i = 0; i < a.Favorites.Count(); i++)
            {
                novels.Add(a.Favorites[i].Novel);
            }
            return novels;
        }
        public bool AddFavorite(int userid, int novelid)
        {
            try
            {
                _context.Favorites.Add(new() { Novel = _context.Novels.Find(novelid), Account = _context.Accounts.Find(userid) });
                _context.SaveChanges();
                return true;
            }
            catch (InvalidOperationException)
            {
                throw new InvalidOperationException("Error adding novel to favorites");
            }
        }
        public bool RemoveFavorite(int userid, int novelid)
        {
            try
            {
                Favorites f = _context.Favorites.Where(f => f.AccountId == userid && f.NovelId == novelid)
                    .Single();
                _context.Favorites.Remove(f);
                _context.SaveChanges();
                return true;
            }
            catch (InvalidOperationException)
            {
                throw new InvalidOperationException("Error removing novel from favorites");
            }
        }
    }
}
