using System;
using System.Data.Entity;
using System.Diagnostics.CodeAnalysis;
using System.Threading.Tasks;
using MCPImporter.Common.Entities;

namespace MCPImporter.Data
{
    #region Interface

    public interface IMCPImporterUnitOfWork : IDisposable, IUnitOfWork
    {
        IRepository<AssignedCompetency> AssignedCompetencyRepository { get; }
        IRepository<CertificationName> CertificationNameRepository { get; }
        IRepository<CertificationTrack> CertificationTrackRepository { get; }
        IRepository<Location> LocationRepository { get; }
        IRepository<Organization> OrganizationRepository { get; }
        IRepository<Person> PersonRepository { get; }
        IRepository<QualifyingCompetency> QualifyingCompetencyRepository { get; }
        IRepository<Status> StatusRepository { get; }

        void InitializeDatabase();

        void EnableLazyLoading(bool isEnabled);
    }

    #endregion

    [ExcludeFromCodeCoverage]
    public partial class MCPImporterUnitOfWork : IMCPImporterUnitOfWork
    {
        private readonly MCPImporterContainer _context;

        private readonly Lazy<IRepository<AssignedCompetency>> _assignedCompetencyRepository;
        private readonly Lazy<IRepository<CertificationName>> _certificationNameRepository;
        private readonly Lazy<IRepository<CertificationTrack>> _certificationTrackRepository;
        private readonly Lazy<IRepository<Location>> _locationRepository;
        private readonly Lazy<IRepository<Organization>> _organizationRepository;
        private readonly Lazy<IRepository<Person>> _personRepository;
        private readonly Lazy<IRepository<QualifyingCompetency>> _qualifyingCompetencyRepository;
        private readonly Lazy<IRepository<Status>> _statusRepository;

        public virtual IRepository<AssignedCompetency> AssignedCompetencyRepository
        {
            get { return _assignedCompetencyRepository.Value; }
        }

        public virtual IRepository<CertificationName> CertificationNameRepository
        {
            get { return _certificationNameRepository.Value; }
        }

        public virtual IRepository<CertificationTrack> CertificationTrackRepository
        {
            get { return _certificationTrackRepository.Value; }
        }

        public virtual IRepository<Location> LocationRepository
        {
            get { return _locationRepository.Value; }
        }

        public virtual IRepository<Organization> OrganizationRepository
        {
            get { return _organizationRepository.Value; }
        }

        public virtual IRepository<Person> PersonRepository
        {
            get { return _personRepository.Value; }
        }

        public virtual IRepository<QualifyingCompetency> QualifyingCompetencyRepository
        {
            get { return _qualifyingCompetencyRepository.Value; }
        }

        public virtual IRepository<Status> StatusRepository
        {
            get { return _statusRepository.Value; }
        }

        public MCPImporterUnitOfWork()
            : this(new MCPImporterContainer())
        {
        }

        public MCPImporterUnitOfWork(MCPImporterContainer context)
        {
            _context = context;

            _assignedCompetencyRepository = GetLazyRepository<AssignedCompetency>(_context);
            _certificationNameRepository = GetLazyRepository<CertificationName>(_context);
            _certificationTrackRepository = GetLazyRepository<CertificationTrack>(_context);
            _locationRepository = GetLazyRepository<Location>(_context);
            _organizationRepository = GetLazyRepository<Organization>(_context);
            _personRepository = GetLazyRepository<Person>(_context);
            _qualifyingCompetencyRepository = GetLazyRepository<QualifyingCompetency>(_context);
            _statusRepository = GetLazyRepository<Status>(_context);
        }

        private static Lazy<IRepository<TEntity>> GetLazyRepository<TEntity>(DbContext context) where TEntity : class
        {
            return new Lazy<IRepository<TEntity>>(() => new EntityRepository<TEntity>(context));
        }

        public virtual void Save()
        {
            _context.SaveChanges();
        }

        //public async virtual Task Save()
        //{
        //    await _context.SaveChanges();
        //}

        public virtual Task SaveAsync()
        {
            return _context.SaveChangesAsync();
        }

        public void InitializeDatabase()
        {
            _context.InitializeDatabase();
        }

        public void EnableLazyLoading(bool isEnabled)
        {
            this._context.Configuration.LazyLoadingEnabled = isEnabled;
        }

        #region IDisposable Methods

        private bool disposed = false;

        protected virtual void Dispose(bool disposing)
        {
            if (!this.disposed)
            {
                if (disposing)
                {
                    _context.Dispose();
                }
            }
            this.disposed = true;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        #endregion
    }
}
