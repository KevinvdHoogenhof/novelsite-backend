using API.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.Data
{
    public interface INovelContext
    {
        public DbSet<Novel> Novels { get; set; }
        public DbSet<Genre> Genres { get; set; }
        public DbSet<Account> Accounts { get; set; }
        public DbSet<NovelGenre> NovelGenres { get; set; }
        public DbSet<Favorites> Favorites { get; set; }
        public DbSet<Role> Roles { get; set; }
        int SaveChanges();
    }
}
