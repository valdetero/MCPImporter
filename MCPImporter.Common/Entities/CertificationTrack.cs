using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace MCPImporter.Common.Entities
{
    public partial class CertificationTrack : ILookupValue
    {
        public CertificationTrack()
        {
            
        }

        public int Id { get; set; }
        [MaxLength(25)]
        public string Name { get; set; }

        public virtual ICollection<Person> People { get; set; }
    }
}
