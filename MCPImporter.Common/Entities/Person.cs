using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MCPImporter.Common.Entities
{
    public partial class Person
    {
        public Person()
        {
            this.CertificationNames = new HashSet<CertificationName>();
            this.CertificationTracks = new HashSet<CertificationTrack>();
            this.QualifyingCompetencies = new HashSet<QualifyingCompetency>();
        }

        public int Id { get; set; }
        [MaxLength(25)]
        public string MCPId { get; set; }
        [MaxLength(25)]
        public string LastName { get; set; }
        [MaxLength(25)]
        public string FirstName { get; set; }
        [MaxLength(100)]
        public string Email { get; set; }

        public virtual Location Location { get; set; }
        public virtual Status Status { get; set; }
        public virtual AssignedCompetency AssignedCompetency { get; set; }

        public virtual ICollection<CertificationTrack> CertificationTracks { get; set; }
        public virtual ICollection<CertificationName> CertificationNames { get; set; }
        public virtual ICollection<QualifyingCompetency> QualifyingCompetencies { get; set; } 
    }
}
