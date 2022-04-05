using API.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.ViewModels
{
    public class NovelViewModel
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Author { get; set; }

        public string CoverImage { get; set; }
        public string Description { get; set; }
        public List<GenreViewModel> Genres { get; set; }
        public NovelViewModel(Novel novel)
        {
            Id = novel.Id;
            Title = novel.Title;
            Author = novel.Author;
            CoverImage = novel.CoverImage;
            Description = novel.Description;
            Genres = new();
            if (novel.Genres != null)
            {
                for (int i = 0; i < novel.Genres.Count; i++)
                {
                    Genres.Add(new(novel.Genres[i].Genre));
                }

            }
        }
    }
}
