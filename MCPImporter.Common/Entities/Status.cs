using System.ComponentModel.DataAnnotations;

namespace MCPImporter.Common.Entities
{
    public partial class Status : ILookupValue
    {
        public Status()
        {
            
        }

        public int Id { get; set; }
        [MaxLength(25)]
        public string Name { get; set; }
    }
}
