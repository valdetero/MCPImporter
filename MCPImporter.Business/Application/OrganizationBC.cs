using MCPImporter.Common.Entities;
using MCPImporter.Data;

namespace MCPImporter.Business.Application
{
    public class OrganizationBC : ApplicationBCBase<Organization>
    {
        public OrganizationBC(IMCPImporterUnitOfWork uow)
            : base(uow)
        {

        }

        public OrganizationBC()
            : base()
        {

        }

        internal override IRepository<Organization> repository
        {
            get { return uow.OrganizationRepository; }
        }
    }
}
