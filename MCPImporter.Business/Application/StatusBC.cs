using MCPImporter.Common.Entities;
using MCPImporter.Data;

namespace MCPImporter.Business.Application
{
    public class StatusBC : ApplicationBCBase<Status>
    {
        public StatusBC(IMCPImporterUnitOfWork uow)
            : base(uow)
        {

        }

        public StatusBC()
            : base()
        {

        }

        internal override IRepository<Status> repository
        {
            get { return uow.StatusRepository; }
        }
    }
}
