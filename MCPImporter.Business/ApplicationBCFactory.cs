using System;
using MCPImporter.Business.Application;
using MCPImporter.Common.Entities;
using MCPImporter.Data;

namespace MCPImporter.Business
{
    public class ApplicationBCFactory : BaseBCFactory, IApplicationBCFactory
    {
        private readonly Lazy<AssignedCompetencyBC> _assignedCompetencyBC;
        private readonly Lazy<CertificationNameBC> _certificationNameBC;
        private readonly Lazy<CertificationTrackBC> _certificationTrackBC;
        private readonly Lazy<LocationBC> _locationBC;
        private readonly Lazy<OrganizationBC> _organizationBC;
        private readonly Lazy<PersonBC> _personBC;
        private readonly Lazy<QualifyingCompetencyBC> _qualifyingCompetencyBC;
        private readonly Lazy<StatusBC> _statusBC;

        public ApplicationBCFactory(IMCPImporterUnitOfWork uow)
            : base(uow)
        {
            _assignedCompetencyBC = new Lazy<AssignedCompetencyBC>(() => new AssignedCompetencyBC(this.uow));
            _certificationNameBC = new Lazy<CertificationNameBC>(() => new CertificationNameBC(this.uow));
            _certificationTrackBC = new Lazy<CertificationTrackBC>(() => new CertificationTrackBC(this.uow));
            _locationBC = new Lazy<LocationBC>(() => new LocationBC(this.uow));
            _organizationBC = new Lazy<OrganizationBC>(() => new OrganizationBC(this.uow));
            _personBC = new Lazy<PersonBC>(() => new PersonBC(this.uow));
            _qualifyingCompetencyBC = new Lazy<QualifyingCompetencyBC>(() => new QualifyingCompetencyBC(this.uow));
            _statusBC = new Lazy<StatusBC>(() => new StatusBC(this.uow));
        }

        public ApplicationBCFactory()
            : this(new MCPImporterUnitOfWork())
        {

        }

        public IEntityBC<AssignedCompetency> AssignedCompetencyBC
        {
            get { return _assignedCompetencyBC.Value; }
        }

        public IEntityBC<CertificationName> CertificationNameBC
        {
            get { return _certificationNameBC.Value; }
        }

        public IEntityBC<Location> LocationBC
        {
            get { return _locationBC.Value; }
        }

        public IEntityBC<CertificationTrack> CertificationTrackBC
        {
            get { return _certificationTrackBC.Value; }
        }

        public IEntityBC<Organization> OrganizationBC
        {
            get { return _organizationBC.Value; }
        }

        public IEntityBC<Person> PersonBC
        {
            get { return _personBC.Value; }
        }

        public IEntityBC<QualifyingCompetency> QualifyingCompetencyBC
        {
            get { return _qualifyingCompetencyBC.Value; }
        }

        public IEntityBC<Status> StatusBC
        {
            get { return _statusBC.Value; }
        }
    }
}
