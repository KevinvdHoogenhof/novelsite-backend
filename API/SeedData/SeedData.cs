using API.Data;
using API.Models;
using API.Services;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Threading.Tasks;

namespace API.SeedData
{
    public static class SeedData
    {
        private static IEncryptionService _encrypt = new EncryptionService();

        public static void Initialize(IServiceProvider serviceProvider)
        {
            using (var context = new NovelContext(serviceProvider.GetRequiredService<DbContextOptions<NovelContext>>()))
            {
                if (!context.Roles.Any())
                {
                    context.Roles.Add(new Role() { Name = "Standard" });
                    context.SaveChanges();
                    context.Roles.Add(new Role() { Name = "Admin" });
                    context.SaveChanges();
                }
                // Look for any accounts 
                if (!context.Accounts.Any())
                {
                    ////
                    var hashsalt = _encrypt.EncryptPassword("pw1");
                    context.Accounts.Add(new Account() { Name = "name1", Email = "email1", Password = hashsalt.Hash, StoredSalt = hashsalt.Salt, Role = context.Roles.Find(1) });
                    context.SaveChanges();

                    ////
                    var hashsalt2 = _encrypt.EncryptPassword("admin");
                    context.Accounts.Add(new Account() { Name = "admin", Email = "admin", Password = hashsalt2.Hash, StoredSalt = hashsalt2.Salt, Role = context.Roles.Find(2) });
                    context.SaveChanges();

                    ////
                    var hashsalt3 = _encrypt.EncryptPassword("pw3");
                    context.Accounts.Add(new Account() { Name = "name3", Email = "email3", Password = hashsalt3.Hash, StoredSalt = hashsalt3.Salt, Role = context.Roles.Find(1) });
                    context.SaveChanges();
                }
                if (!context.Novels.Any())
                {
                    context.Novels.Add(new Novel() { Title = "Novel1", Author = "author1", CoverImage = "coverimage1", Description = "novel 1 description" });
                    context.SaveChanges();
                    context.Novels.Add(new Novel() { Title = "Novel2", Author = "author2", CoverImage = "coverimage2", Description = "description of novel 2" });
                    context.SaveChanges();
                    context.Novels.Add(new Novel() { Title = "Novel3", Author = "author1", CoverImage = "coverimage3", Description = "second novel of author1" });
                    context.SaveChanges();
                    context.Novels.Add(new Novel() { Title = "Novel4", Author = "author3", CoverImage = "coverimage4", Description = "description of novel #4" });
                    context.SaveChanges();
                }
                if (!context.Genres.Any())
                {
                    context.Genres.Add(new Genre(){ Name = "Action" });
                    context.SaveChanges();
                    context.Genres.Add(new Genre(){ Name = "Adventure" });
                    context.SaveChanges();
                    context.Genres.Add(new Genre(){ Name = "Comedy" });
                    context.SaveChanges();
                    context.Genres.Add(new Genre(){ Name = "Drama" });
                    context.SaveChanges();
                    context.Genres.Add(new Genre(){ Name = "Fantasy" });
                    context.SaveChanges();
                    context.Genres.Add(new Genre(){ Name = "Historical" });
                    context.SaveChanges();
                    context.Genres.Add(new Genre(){ Name = "Romance" });
                    context.SaveChanges();
                }
                if (!context.NovelGenres.Any())
                { 
                    // Novel 1
                    context.NovelGenres.Add(new() { Novel = context.Novels.Find(1), Genre = context.Genres.Find(1) });
                    context.SaveChanges();
                    context.NovelGenres.Add(new() { Novel = context.Novels.Find(1), Genre = context.Genres.Find(2) });
                    context.SaveChanges();
                    // Novel 2
                    context.NovelGenres.Add(new() { Novel = context.Novels.Find(2), Genre = context.Genres.Find(2) });
                    context.SaveChanges();
                    // Novel 3
                    context.NovelGenres.Add(new() { Novel = context.Novels.Find(3), Genre = context.Genres.Find(2) });
                    context.SaveChanges();
                    context.NovelGenres.Add(new() { Novel = context.Novels.Find(3), Genre = context.Genres.Find(3) });
                    context.SaveChanges();
                    context.NovelGenres.Add(new() { Novel = context.Novels.Find(3), Genre = context.Genres.Find(6) });
                    context.SaveChanges();
                    // Novel 4
                    context.NovelGenres.Add(new() { Novel = context.Novels.Find(4), Genre = context.Genres.Find(4) });
                    context.SaveChanges();
                    context.NovelGenres.Add(new() { Novel = context.Novels.Find(4), Genre = context.Genres.Find(5) });
                    context.SaveChanges();
                    context.NovelGenres.Add(new() { Novel = context.Novels.Find(4), Genre = context.Genres.Find(7) });
                    context.SaveChanges();
                }
            }
        }
    }
}
