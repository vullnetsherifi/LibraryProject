using Library.Api.Entities;
using LibraryFinalProject.src.Library.Api.Entities;
using Microsoft.EntityFrameworkCore;

namespace Library.Api.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    public DbSet<User> Users => Set<User>();
    public DbSet<Book> Books => Set<Book>();
    public DbSet<Category> Categories => Set<Category>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Category>()
            .HasKey(c => c.Slug);

        modelBuilder.Entity<Category>()
            .HasIndex(c => c.Name)
            .IsUnique();

        modelBuilder.Entity<Book>()
            .HasOne(b => b.Category)
            .WithMany(c => c.Books)
            .HasForeignKey(b => b.CategorySlug)
            .HasPrincipalKey(c => c.Slug)
            .OnDelete(DeleteBehavior.Restrict);
    }

}
