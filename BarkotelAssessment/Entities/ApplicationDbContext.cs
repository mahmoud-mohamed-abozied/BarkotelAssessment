using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BarkotelAssessment.Entities
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<AuthorBook>()
               .HasKey(x => new { x.AuthorId, x.BookId });

            modelBuilder.Entity<AuthorBook>()
                .HasOne(pt => pt.Book)
                .WithMany(t => t.AuthorBook)
                .HasForeignKey(pt => pt.AuthorId);

            modelBuilder.Entity<AuthorBook>()
                .HasOne(pt => pt.Book)
                .WithMany(p => p.AuthorBook)
                .HasForeignKey(pt => pt.BookId);

        }
        public DbSet<Author> Authors { get; set; }
        public DbSet<Book> Books { get; set; }
        public DbSet<AuthorBook> AuthorBooks { get; set; }
        

    }
}
