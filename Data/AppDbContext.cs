using Microsoft.EntityFrameworkCore;
using ProductionTimeAnalyzer.Models;


namespace ProductionTimeAnalyzer.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<Product> Products => Set<Product>();
        public DbSet<Machine> Machines => Set<Machine>();
        public DbSet<TimeEntry> TimeEntries => Set<TimeEntry>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            ConfigureProduct(modelBuilder);
            ConfigureMachine(modelBuilder);
            ConfigureTimeEntry(modelBuilder);
        }
        private static void ConfigureProduct(ModelBuilder modelBuilder) {
            modelBuilder.Entity<Product>(entity =>
            {
                entity.ToTable("Products");
                entity.HasKey(p => p.Id);
                entity.Property(p => p.Name)
                .IsRequired()
                .HasMaxLength(200);
                entity.Property(p => p.OrderNumber)
                .HasMaxLength(100);
            });
    }

        private static void ConfigureMachine(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Machine>(entity =>
            {
                entity.ToTable("Machines");

                entity.HasKey(m => m.Id);

                entity.Property(m => m.Name)
                      .IsRequired()
                      .HasMaxLength(200);

                entity.Property(m => m.Type)
                      .IsRequired()
                      .HasMaxLength(100);
            });
        }

        private static void ConfigureTimeEntry(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<TimeEntry>(entity =>
            {
                entity.ToTable("TimeEntries");

                entity.HasKey(t => t.Id);

                entity.Property(t => t.StartTime)
                      .IsRequired();

                entity.Property(t => t.EndTime)
                      .IsRequired();

                entity.Property(t => t.Status)
                      .IsRequired();

                entity.HasOne(t => t.Product)
                      .WithMany(p => p.TimeEntries)
                      .HasForeignKey(t => t.ProductId)
                      .OnDelete(DeleteBehavior.Restrict);

                entity.HasOne(t => t.Machine)
                      .WithMany(m => m.TimeEntries)
                      .HasForeignKey(t => t.MachineId)
                      .OnDelete(DeleteBehavior.Restrict);
            });
        }
    }


}
