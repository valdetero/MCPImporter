using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace MCPImporter.Common.Entities
{
    public partial class CertificationName : ILookupValue
    {
        public CertificationName()
        {
            
        }

        public int Id { get; set; }
        [MaxLength(150)]
        public string Name { get; set; }

        public virtual ICollection<Person> People { get; set; }
    }
}
