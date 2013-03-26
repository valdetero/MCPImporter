using System.ComponentModel.DataAnnotations;

namespace MCPImporter.Common.Entities
{
    public partial class Organization
    {
        public Organization()
        {
            
        }

        public int Id { get; set; }
        [MaxLength(25)]
        public string OrgPartnerId { get; set; }
        
    }
}
