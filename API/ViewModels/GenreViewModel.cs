using API.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.ViewModels
{
    public class GenreViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public GenreViewModel(Genre genre)
        {
            Id = genre.Id;
            Name = genre.Name;
        }
    }
}
