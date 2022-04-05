using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.Models
{
    public class Novel
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Author { get; set; }

        public string CoverImage { get; set; }
        public string Description { get; set; }
        public IList<NovelGenre> Genres { get; set; }
        public IList<Favorites> Favorites { get; set; }
    }
}
