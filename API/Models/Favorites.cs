using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.Models
{
    public class Favorites
    {
        public int AccountId { get; set; }
        public Account Account { get; set; }
        public int NovelId { get; set; }
        public Novel Novel { get; set; }

        public int CurrentChapter { get; set; }
    }
}
