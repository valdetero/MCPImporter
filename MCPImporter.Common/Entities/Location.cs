using System.ComponentModel.DataAnnotations;

namespace MCPImporter.Common.Entities
{
    public partial class Location
    {
        public Location()
        {
            
        }

        public int Id { get; set; }
        [MaxLength(25)]
        public string PartnerId { get; set; }
        [MaxLength(100)]
        public string Name { get; set; }
        [MaxLength(50)]
        public string Country { get; set; }

        public virtual Organization Organization { get; set; }
    }
}
