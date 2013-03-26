using MCPImporter.Common.Entities;
using MCPImporter.Data;

namespace MCPImporter.Business.Application
{
    public class QualifyingCompetencyBC : ApplicationBCBase<QualifyingCompetency>
    {
        public QualifyingCompetencyBC(IMCPImporterUnitOfWork uow)
            : base(uow)
        {

        }

        public QualifyingCompetencyBC()
            : base()
        {

        }

        internal override IRepository<QualifyingCompetency> repository
        {
            get { return uow.QualifyingCompetencyRepository; }
        }
    }
}
