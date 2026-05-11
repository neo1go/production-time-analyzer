using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using ProductionTimeAnalyzer.Models;

namespace ProductionTimeAnalyzer.Data;

public class ProductionTimeAnalyzerContext : IdentityDbContext<IdentityUser>
{
    public ProductionTimeAnalyzerContext(DbContextOptions<ProductionTimeAnalyzerContext> options)
        : base(options)
    {
    }

    public DbSet<Product> Products => Set<Product>();
    public DbSet<Machine> Machines => Set<Machine>();
    public DbSet<TimeEntry> TimeEntries => Set<TimeEntry>();
    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);
        // Customize the ASP.NET Identity model and override the defaults if needed.
        // For example, you can rename the ASP.NET Identity table names and more.
        // Add your customizations after calling base.OnModelCreating(builder);
    }
}
