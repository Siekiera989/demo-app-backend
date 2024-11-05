using Microsoft.EntityFrameworkCore;

namespace DemoApp.DataAccessLayer.DbContext;

public class PostgreDbContext : Microsoft.EntityFrameworkCore.DbContext
{
    public PostgreDbContext(DbContextOptions<PostgreDbContext> options) : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
    }
}