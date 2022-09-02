using Microsoft.EntityFrameworkCore;

public class A1DBContext : DbContext
{
    public A1DBContext(DbContextOptions<A1DBContext> options) : base(options) { }
    public DbSet<Comment> Comments { get; set; }
    public DbSet<Product> Products { get; set; }


    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseSqlite("Data Source=A1Database.sqlite");
    }
}

