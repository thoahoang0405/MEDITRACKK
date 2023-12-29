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
    [Table("appointments")]
    public class AppointmentEntity
    {
        [Key]
        public Guid AppointmentID { get; set; }

        [ForeignKey(nameof(UserID))]
        public Guid? UserID { get; set; }
        public string? AppointmentName { get; set; }
        public DateTime? AppointmentDate { get; set; }
        public int? AppointmentStatus { get; set; }
        public string? PatientName { get; set;}
        public Guid? PatientID { get; set; }
        public string? DoctorName { get; set; }
        public string? DoctorPhoneNumber { get; set;}
        public string? Address { get; set; }
        public string? Description { get; set; }
        public DateTime? CreatedDate { get; set; }
        public string? CreatedBy { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public string? ModifiedBy { get; set; }
        public List<NotificationsEntity> Notice { get; set; }


    }
}
