using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace MCPImporter.Common.Entities
{
    public partial class QualifyingCompetency : ILookupValue
    {
        public QualifyingCompetency()
        {
            
        }

        public int Id { get; set; }
        [MaxLength(100)]
        public string Name { get; set; }

        public virtual ICollection<Person> People { get; set; }
    }
}
