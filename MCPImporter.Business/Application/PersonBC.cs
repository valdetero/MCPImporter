using System.Threading.Tasks;
using MCPImporter.Common.Entities;
using MCPImporter.Data;

namespace MCPImporter.Business.Application
{
    public class PersonBC : ApplicationBCBase<Person>
    {
        public PersonBC(IMCPImporterUnitOfWork uow) : base(uow) { }

        public PersonBC() { }

        internal override IRepository<Person> repository
        {
            get { return uow.PersonRepository; }
        }



    }
}
