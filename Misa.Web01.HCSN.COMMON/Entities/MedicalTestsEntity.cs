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
    [Table("medicaltests")]
    public class MedicalTestsEntity
    {
        [Key]
        public Guid TestID { get; set; }
        public Guid RecordID { get; set; }
        public string? TestName { get; set; }
        public string? Unit { get; set; }
        public string? Normal { get; set; }
        public DateTime? TestDate { get; set; }
        public DateTime? SamplingTime { get; set; }
        public DateTime? SampleReceiptTime { get; set; }
        public string? TypeTest { get; set; }
        public string? TestBy { get; set; }
        public string? TestResult { get; set; }
        public DateTime? CreatedDate { get; set; }
        public string? CreatedBy { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public string? ModifiedBy { get; set; }



    }
}
