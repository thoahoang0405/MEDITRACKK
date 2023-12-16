using CsQuery.Engine.PseudoClassSelectors;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MEDITRACK.COMMON.Entities
{
    [Table("family_members")]
    public class FamilyMembersEntity
    {
        [Key]
        public Guid PatientID { get; set; }
        public Guid UserID { get; set; }
        public string PatientName { get; set; }
        public DateTime DateOfBirth { get; set; }
        public string Relationship { get; set; }
        public string PhoneNumber { get; set; }
        public string Contact { get; set; }
        public DateTime? CreatedDate { get; set; }
        public string? CreatedBy { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public string? ModifiedBy { get; set; }

    }
}
