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
    [Table("prescriptions")]
    public class PrescriptionEntity
    {
        [Key]
        public Guid PrescriptionID { get; set; }
        public Guid UserID { get;  set; }
        public string? PrescriptionName { get; set; }
        public string? Notes { get; set; }
        public int PrescriptionStatus { get; set; }
        public DateTime? PrescriptionDate { get; set; }
        public string? PatientName { get; set; }
        public Guid? PatientID { get; set;}
        public string? CreatedByDoctor { get; set; }
        public string? Diagnose { get; set;}
        public string? CreatedByAddress { get; set; }
        public DateTime? FromDate { get; set; }
        public DateTime? ToDate { get; set; }
        public DateTime? CreatedDate { get; set;}=DateTime.Now;
        public string? CreatedBy { get; set; }
        public DateTime? ModifiedDate { get; set; }=DateTime.Now;
        public string?   ModifiedBy { get; set; }

        public List<MedicationsEntity> Medications { get; set; }

    }
}
