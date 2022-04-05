using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.Models
{
    public class Genre
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public IList<NovelGenre> Novels { get; set; }
    }
}
