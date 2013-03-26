using System.ComponentModel.DataAnnotations;

namespace MCPImporter.Common.Entities
{
    public partial class AssignedCompetency : ILookupValue
    {
        public AssignedCompetency()
        {
            
        }

        public int Id { get; set; }
        [MaxLength(100)]
        public string Name { get; set; }
    }
}
