using Microsoft.EntityFrameworkCore;

public class PasswordDbContext : DbContext
{
    public DbSet<Password> Passwords { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseSqlite("Data Source=pwds.db");
    }
}
