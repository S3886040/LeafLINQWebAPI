using LeafLINQWebAPI.Model;
using Microsoft.EntityFrameworkCore;

namespace WebApi.Data;

public class LeafLINQContext : DbContext
{
    public LeafLINQContext(DbContextOptions<LeafLINQContext> options) : base(options)
    { }

    public DbSet<User> User { get; set; } 

    public DbSet<Plant> Plant { get; set; }

    public DbSet<Setting> Setting { get; set; }

    public DbSet<TempUser> TempUser { get; set; }
    public DbSet<Session> Session { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Plant>()
            .HasOne(p => p.User)
            .WithMany(u => u.Plants)
            .HasForeignKey(p => p.UserId);
    }

}