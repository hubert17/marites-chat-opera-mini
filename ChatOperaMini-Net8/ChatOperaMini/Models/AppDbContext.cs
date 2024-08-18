using Microsoft.EntityFrameworkCore;

namespace ChatOperaMini.Models;

/*
 * === EF Core Db Migration ===
 * PM> Add-Migration <migration_name>
 * PM> Update-Database
 * */

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Message>().HasData(Message.SeedData());
        modelBuilder.Entity<MessageRead>().HasData(MessageRead.SeedData());
    }

    #region DbSets
    public DbSet<Message> Messages { get; set; }
    public DbSet<MessageRead> MessageReads { get; set; }
    public DbSet<PushSubscription> PushSubscriptions { get; set; }
    #endregion
}
