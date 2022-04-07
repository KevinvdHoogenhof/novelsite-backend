using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace API.Models
{
    [Table("Novels")]
    public class Novel
    {
        [Key]
        public int Id { get; set; }
        [MaxLength(200)]
        public string Title { get; set; }
        [MaxLength(200)]
        public string Author { get; set; }

        public string CoverImage { get; set; }
        public string Description { get; set; }
        public IList<NovelGenre> Genres { get; set; }
        public IList<Favorites> Favorites { get; set; }
        public IList<Comment> Comments { get; set; }
    }
}
