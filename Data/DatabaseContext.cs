using AMDB.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AMDB.Data
{
    public class DatabaseContext : DbContext
    {
        protected override void OnConfiguring(DbContextOptionsBuilder options)
        {
            options.UseSqlite("Data Source=Data/Database.db");
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<ProductionGenre>()
                .HasKey(pp => new { pp.ProductionId, pp.GenreId });
            modelBuilder.Entity<ProductionGenre>()
                .HasOne(pg => pg.Genre)
                .WithMany(g => g.Productions)
                .HasForeignKey(pg => pg.GenreId);
            modelBuilder.Entity<ProductionGenre>()
                .HasOne(pp => pp.Production)
                .WithMany(p => p.Genres)
                .HasForeignKey(pp => pp.ProductionId);

            modelBuilder.Entity<ProductionPerson>()
                .HasKey(pp => new { pp.ProductionId, pp.PersonId });
            modelBuilder.Entity<ProductionPerson>()
                .HasOne(pp => pp.Person)
                .WithMany(p => p.ProductionCredits)
                .HasForeignKey(pp => pp.PersonId);
            modelBuilder.Entity<ProductionPerson>()
                .HasOne(pp => pp.Production)
                .WithMany(p => p.Stars)
                .HasForeignKey(pp => pp.ProductionId);

            modelBuilder.Entity<DirectorPerson>()
                .HasKey(pp => new { pp.ProductionId, pp.PersonId });
            modelBuilder.Entity<DirectorPerson>()
                .HasOne(pp => pp.Person)
                .WithMany(p => p.DirectorCredits)
                .HasForeignKey(pp => pp.PersonId);
            modelBuilder.Entity<DirectorPerson>()
                .HasOne(pp => pp.Production)
                .WithOne(p => p.Director);

            modelBuilder.Entity<ProductionKeyword>()
                .HasKey(pt => new { pt.ProductionId, pt.KeywordId });
            modelBuilder.Entity<ProductionKeyword>()
                .HasOne(pt => pt.Keyword)
                .WithMany(t => t.Productions)
                .HasForeignKey(pt => pt.KeywordId);
            modelBuilder.Entity<ProductionKeyword>()
                .HasOne(pt => pt.Production)
                .WithMany(p => p.Keywords)
                .HasForeignKey(pt => pt.ProductionId);

            modelBuilder.Entity<UserRating>()
                .HasKey(pt => new { pt.RatingId, pt.UserId });
            modelBuilder.Entity<UserRating>()
                .HasOne(e => e.Rating)
                .WithMany(c => c.UserRatings)
                .HasForeignKey(u => u.RatingId);
            modelBuilder.Entity<UserRating>()
                .HasOne(e => e.User)
                .WithMany(c => c.ProductionRatings)
                .HasForeignKey(u => u.UserId);
        }
        public DbSet<Person> Persons { get; set; }
        public DbSet<Production> Productions { get; set; }
        public DbSet<ProductionPerson> ProductionPersonLinks { get; set; }
        public DbSet<ProductionGenre> ProductionGenreLinks { get; set; }
        public DbSet<ProductionKeyword> ProductionKeywordLinks { get; set; }
        public DbSet<DirectorPerson> DirectorPersonLinks { get; set; }
        public DbSet<Movie> Movies { get; set; }
        public DbSet<TVShow> TVShows { get; set; }
        public DbSet<Keyword> Keywords { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Rating> Ratings { get; set; }
        public DbSet<UserRating> UserRatings { get; set; }
        public DbSet<Genre> Genres { get; set; }
    }
}
