using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MCPImporter.Common.Entities
{
    public interface ILookupValue
    {
        int Id { get; set; }
        string Name { get; set; }
    }
}
