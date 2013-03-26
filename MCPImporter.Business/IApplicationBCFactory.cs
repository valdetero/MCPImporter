using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MCPImporter.Business.Application;
using MCPImporter.Common.Entities;

namespace MCPImporter.Business
{
    public interface IApplicationBCFactory
    {
        IEntityBC<AssignedCompetency> AssignedCompetencyBC { get; }
        IEntityBC<CertificationName> CertificationNameBC { get; }
        IEntityBC<Location> LocationBC { get; }
        IEntityBC<CertificationTrack> CertificationTrackBC { get; }
        IEntityBC<Organization> OrganizationBC { get; }
        IEntityBC<Person> PersonBC { get; }
        IEntityBC<QualifyingCompetency> QualifyingCompetencyBC { get; }
        IEntityBC<Status> StatusBC { get; }
    }
}
