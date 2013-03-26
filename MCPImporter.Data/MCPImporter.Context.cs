using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;
using System.Threading.Tasks;
using MCPImporter.Common.Entities;

namespace MCPImporter.Data
{
    public class MCPImporterContainer : DbContext
    {
        public MCPImporterContainer()
            : base("name=MCPImporterContainer")
        {
            
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();

            //force tables to be named what we want
            modelBuilder.Entity<Person>()
                .HasMany(p => p.CertificationNames)
                .WithMany(c => c.People)
                .Map(m =>
                {
                    m.ToTable("PersonCertificationName");
                    m.MapLeftKey("Person_Id");
                    m.MapRightKey("CertificationName_Id");
                });

            modelBuilder.Entity<Person>()
                .HasMany(p => p.CertificationTracks)
                .WithMany(c => c.People)
                .Map(m =>
                {
                    m.ToTable("PersonCertificationTrack");
                    m.MapLeftKey("Person_Id");
                    m.MapRightKey("CertificationTrack_Id");
                });

            modelBuilder.Entity<Person>()
                .HasMany(p => p.QualifyingCompetencies)
                .WithMany(c => c.People)
                .Map(m =>
                {
                    m.ToTable("PersonQualifyingCompentency");
                    m.MapLeftKey("Person_Id");
                    m.MapRightKey("QualifyingCompentency_Id");
                });

            base.OnModelCreating(modelBuilder);
        }

        public override int SaveChanges()
        {
            ChangeTracker.DetectChanges();

            return base.SaveChanges();
        }

        public async new Task<int> SaveChangesAsync()
        {
            ChangeTracker.DetectChanges();

            return await base.SaveChangesAsync();
        }
    
        public DbSet<Location> Locations { get; set; }
        public DbSet<AssignedCompetency> AssignedCompetencies { get; set; }
        public DbSet<CertificationName> CertificationNames { get; set; }
        public DbSet<CertificationTrack> CertificationTracks { get; set; }
        public DbSet<Organization> Organizations { get; set; }
        public DbSet<QualifyingCompetency> QualifyingCompetencies { get; set; }
        public DbSet<Person> People { get; set; }
        public DbSet<Status> Status { get; set; }

        public void InitializeDatabase()
        {
            this.Database.Initialize(true);
            if (this.Database.Exists())
            {
                this.Database.Delete();
            }
            this.Database.Create();
        }
    }
}
