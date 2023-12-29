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
    [Table("treatments")]
    public class TreatmentsEntity
    {
        [Key]
        public Guid TreatmentID { get; set; }
        public Guid RecordID { get; set; }
        public string? Diagnosis { get; set; }
        public DateTime? TreatmentDate { get; set; }
        public string? TreatmentDescription { get; set; }
        public string? MedicalHistory { get; set; }
        public string? PrescriptionName { get; set; }
        public string? Notes { get; set; }
        public DateTime? CreatedDate { get; set; }
        public string? CreatedBy { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public string? ModifiedBy { get; set; }
    }
}
