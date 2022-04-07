using API.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.Data
{
    public class NovelContext : DbContext , INovelContext
    {
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<NovelGenre>().HasKey(ng => new { ng.NovelId, ng.GenreId });
            modelBuilder.Entity<Favorites>().HasKey(fv => new { fv.AccountId, fv.NovelId });
        }
        public NovelContext(DbContextOptions<NovelContext> options)
        : base(options)
        {
        }

        /*protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(@"Server=.\SQLEXPRESS;Database=NovelDB;Trusted_Connection=True;");
        }*/
        public DbSet<Novel> Novels { get; set; }
        public DbSet<Comment> Comments { get; set; }
        public DbSet<Genre> Genres { get; set; }
        public DbSet<Account> Accounts { get; set; }
        public DbSet<NovelGenre> NovelGenres { get; set; }
        public DbSet<Favorites> Favorites { get; set; }
        public DbSet<Role> Roles { get; set; }
    }
}
