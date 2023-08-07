using Microsoft.EntityFrameworkCore;

namespace TestWorkPolyclinic.Models
{
    public partial class PolyclinicDB : DbContext
    {
        public PolyclinicDB() { }

        public PolyclinicDB(DbContextOptions<PolyclinicDB> options) : base(options) { }

        public virtual DbSet<Cabinet> Cabinets { get; set; }
        public virtual DbSet<Doctor> Doctors { get; set; }
        public virtual DbSet<Patient> Patients { get; set; }
        public virtual DbSet<Plot> Plots { get; set; }
        public virtual DbSet<Specialization> Specializations { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder
                    .UseLazyLoadingProxies()
                    .UseNpgsql($"Host={Config.config.DB.Host};" +
                    $"Port={Config.config.DB.Port};" +
                    $"Database={Config.config.DB.Database};" +
                    $"Username={Config.config.DB.Username};" +
                    $"Password={Config.config.DB.Password}");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Doctor>(entity =>
            {
                entity.HasOne(h => h.Cabinet).WithMany(h => h.Doctors).HasForeignKey(h => h.CabinetId);
                entity.HasOne(h => h.Specialization).WithMany(h => h.Doctors).HasForeignKey(h => h.SpecializationId);
                entity.HasOne(h => h.Plot).WithMany(h => h.Doctors).HasForeignKey(h => h.PlotId);
            });

            modelBuilder.Entity<Patient>(entity =>
            {
                entity.HasOne(h => h.Plot).WithMany(h => h.Patients).HasForeignKey(h => h.PlotId);
            });
            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
