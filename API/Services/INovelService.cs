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
        public bool InsertNovel(Novel novel);
    }
}
