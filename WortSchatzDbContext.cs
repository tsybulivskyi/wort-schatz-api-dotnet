using Microsoft.EntityFrameworkCore;
using WordTranslationApp.Models;

public class WortSchatzDbContext : DbContext
{
    public WortSchatzDbContext(DbContextOptions<WortSchatzDbContext> options) : base(options) { }

    public DbSet<Word> Words { get; set; }
    public DbSet<Tag> Tags { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Word>()
            .HasMany(w => w.Tags)
            .WithMany();
        base.OnModelCreating(modelBuilder);
    }
}
