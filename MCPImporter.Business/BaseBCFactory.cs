using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MCPImporter.Data;

namespace MCPImporter.Business
{
    public class BaseBCFactory
    {
        internal IMCPImporterUnitOfWork uow;

        public BaseBCFactory(IMCPImporterUnitOfWork uow)
        {
            this.uow = uow;

            //Initialize Database and seed
            this.uow.InitializeDatabase();
        }
    }
}
