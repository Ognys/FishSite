using Microsoft.EntityFrameworkCore;

public class FishContext(DbContextOptions<FishContext> options) : DbContext(options)
{
    public DbSet<FishModel> Fish { get; set; } 
}