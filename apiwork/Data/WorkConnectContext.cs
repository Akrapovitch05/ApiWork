using Microsoft.EntityFrameworkCore;
using apiwork.Data;
using apiwork.Models;
using MySql.EntityFrameworkCore;


namespace apiwork.Data
{
    public class WorkConnectContext : DbContext
    {
        public WorkConnectContext(DbContextOptions<WorkConnectContext> options)
            : base(options)
        {
        }

        public DbSet<Site> Site { get; set; }
        public DbSet<Service> Service { get; set; }
        public DbSet<Salarie> Salarie { get; set; }
        public DbSet<Admin> Admin { get; set; }
        public DbSet<HistoriqueModification> HistoriqueModification { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Configurations spécifiques
            modelBuilder.Entity<HistoriqueModification>()
                .HasKey(h => h.HistoriqueID);

            modelBuilder.Entity<HistoriqueModification>()
                .HasOne(h => h.Admin)
                .WithMany()
                .HasForeignKey(h => h.AdminID);

            modelBuilder.Entity<HistoriqueModification>()
                .HasOne(h => h.Salarie)
                .WithMany()
                .HasForeignKey(h => h.SalarieID);
        }
    }
}
