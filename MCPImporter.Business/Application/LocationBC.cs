using MCPImporter.Common.Entities;
using MCPImporter.Data;

namespace MCPImporter.Business.Application
{
    public class LocationBC : ApplicationBCBase<Location>
    {
        public LocationBC(IMCPImporterUnitOfWork uow)
            : base(uow)
        {

        }

        public LocationBC()
            : base()
        {

        }

        internal override IRepository<Location> repository
        {
            get { return uow.LocationRepository; }
        }
    }
}
