using Microsoft.EntityFrameworkCore;
using P02_HospitalDatabase.Data.Models;

namespace P02_HospitalDatabase.Data
{
    public class HospitalContext : DbContext
    {
        public HospitalContext()
        {
        }

        public HospitalContext(DbContextOptions options)
            : base(options)
        {
        }

        public virtual DbSet<Diagnose> Diagnoses { get; set; }

        public virtual DbSet<Doctor> Doctors { get; set; }

        public virtual DbSet<Medicament> Medicaments { get; set; }

        public virtual DbSet<Patient> Patients { get; set; }

        public virtual DbSet<PatientMedicament> PatientMedicaments { get; set; }

        public virtual DbSet<Visitation> Visitations { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer("Server=.;Database=HospitalDb;Integrated Security=True");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<PatientMedicament>()
                .HasKey(x => new { x.PatientId, x.MedicamentId });


            base.OnModelCreating(modelBuilder);
        }
    }
}
