using Microsoft.EntityFrameworkCore;

public class A2DBContext : DbContext
{
    public A2DBContext(DbContextOptions<A2DBContext> options) : base(options) { }
    public DbSet<User> Users { get; set; }
    public DbSet<GameRecord> GameRecords { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseSqlite("Data Source=A2Database.sqlite");
    }
}
