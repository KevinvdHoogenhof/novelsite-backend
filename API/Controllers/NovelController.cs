using API.Data;
using API.Services;
using API.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.ViewModels;

namespace API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class NovelController : ControllerBase
    {
        private readonly INovelService _service;
        public NovelController(INovelContext context)
        {
              _service = new NovelService(context);
        }

        [HttpGet]
        public IEnumerable<NovelViewModel> Get()
        {
            List<Novel> novels = _service.GetNovels().ToList();
            List<NovelViewModel> nvms = new();
            for (int i = 0; i < novels.Count(); i++)
            {
                nvms.Add(new(novels[i]));
            }
            return nvms.ToArray();
        }
        [HttpGet("{id}")]
        public NovelViewModel Get(int id)
        {
            Novel n = _service.GetNovel(id);
            return new(n);
        }
        [HttpPost]
        public NovelViewModel Post(string title, string author, string coverimage, string description)
        {
            int id = _service.InsertNovel(new() { Title = title, Author = author, CoverImage = coverimage, Description = description });
            Novel n = _service.GetNovel(id);
            return new(n);
        }
        [HttpDelete("{id}")]
        public bool Delete(int id)
        {
            return _service.DeleteNovel(id);
        }
        [HttpPut("{id}")]
        public bool Put(int id, string title, string author, string coverimage, string description)
        {
            return _service.UpdateNovel(new() { Id = id, Title = title, Author = author, CoverImage = coverimage, Description = description });
        }
    }
}
