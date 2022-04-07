using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace API.Models
{
    [Table("Favorites")]
    public class Favorites
    {
        public int AccountId { get; set; }
        public Account Account { get; set; }
        public int NovelId { get; set; }
        public Novel Novel { get; set; }

        public int CurrentChapter { get; set; }
    }
}
