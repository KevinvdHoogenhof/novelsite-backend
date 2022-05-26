using API.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.Services
{
    public interface INovelService
    {
        public IEnumerable<Novel> GetNovels();
        public Novel GetNovel(int id);
        public int InsertNovel(Novel novel);
        public bool DeleteNovel(int id);
        public bool UpdateNovel(Novel novel);
        public IEnumerable<Novel> GetFavorites(int id);
        public bool AddFavorite(int userid, int novelid);
        public bool RemoveFavorite(int userid, int novelid);
    }
}
