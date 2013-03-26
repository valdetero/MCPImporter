using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MCPImporter.Common.Entities;

namespace MCPImporter.Import
{
    public interface IImporter
    {
        string GetConnectionString(string fileName);
        Task<bool> ExtractInformation(string connectionString);

        Action<string> NumberOfPeopleImported { get; set; }
    }
}
