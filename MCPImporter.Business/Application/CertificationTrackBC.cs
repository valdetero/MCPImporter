using MCPImporter.Common.Entities;
using MCPImporter.Data;

namespace MCPImporter.Business.Application
{
    public class CertificationTrackBC : ApplicationBCBase<CertificationTrack>
    {
        public CertificationTrackBC(IMCPImporterUnitOfWork uow)
            : base(uow)
        {

        }

        public CertificationTrackBC()
            : base()
        {

        }

        internal override IRepository<CertificationTrack> repository
        {
            get { return uow.CertificationTrackRepository; }
        }
    }
}
