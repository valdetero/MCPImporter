using MCPImporter.Common.Entities;
using MCPImporter.Data;

namespace MCPImporter.Business.Application
{
    public class AssignedCompetencyBC : ApplicationBCBase<AssignedCompetency>
    {
        public AssignedCompetencyBC(IMCPImporterUnitOfWork uow)
            : base(uow)
        {

        }

        public AssignedCompetencyBC()
            : base()
        {

        }

        internal override IRepository<AssignedCompetency> repository
        {
            get { return uow.AssignedCompetencyRepository; }
        }
    }
}
