using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.Models
{
    public class NovelGenre
    {
        public int NovelId { get; set; }
        public Novel Novel { get; set; }

        public int GenreId { get; set; }
        public Genre Genre { get; set; }
    }
}
