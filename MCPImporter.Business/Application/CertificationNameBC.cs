using MCPImporter.Common.Entities;
using MCPImporter.Data;

namespace MCPImporter.Business.Application
{
    public class CertificationNameBC : ApplicationBCBase<CertificationName>
    {
        public CertificationNameBC(IMCPImporterUnitOfWork uow)
            : base(uow)
        {

        }

        public CertificationNameBC()
            : base()
        {

        }

        internal override IRepository<CertificationName> repository
        {
            get { return uow.CertificationNameRepository; }
        }
    }
}
