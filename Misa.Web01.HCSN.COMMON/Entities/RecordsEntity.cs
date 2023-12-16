using CsQuery.Engine.PseudoClassSelectors;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace MEDITRACK.COMMON.Entities
{
    [Table("records")]
    public class RecordsEntity
    {
        [Key]
        public Guid RecordID { get; set; }
        public Guid UserID { get; set; }
        public DateTime RecordDate { get; set; }
        public string RecordTitle { get; set; }
        public string MedicalExaminationAddress { get; set; }
        public string DoctorName { get; set; }
        public string FullName { get; set; }
        public DateTime DateOfBirth { get; set; }
        public int Gender { get; set; }
        public string Address { get; set; }
        public string PhoneNumber { get; set; }
        public string DoctorPhoneNumber { get; set; }
        public DateTime? CreatedDate { get; set; }
        public string? CreatedBy { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public string? ModifiedBy { get; set; }
        public string? PatientName { get; set; }
        public Guid? PatientID { get; set; }
        public List<MedicalTestsEntity> MedicalTests { get; set; }
        public List<TreatmentsEntity> Treatments { get; set; }


    }
}
