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
            return _context.Novels
                .Where(n => n.Id == id)
                .Include(n => n.Genres)
                .ThenInclude(n => n.Genre)
                .Include(n => n.Comments)
                .Single();
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
                Novel novel = new() { Id = id };
                _context.Novels.Attach(novel);
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
    }
}
