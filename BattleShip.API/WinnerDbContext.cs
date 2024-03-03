using Microsoft.EntityFrameworkCore;

public class WinnerDbContext : DbContext
{
    public DbSet<Winner> Winners { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseSqlite("Data Source=db/winners.db");
    }
}
