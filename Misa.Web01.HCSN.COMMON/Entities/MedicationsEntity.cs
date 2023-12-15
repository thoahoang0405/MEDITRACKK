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
    [Table("medications")]
    public class MedicationsEntity
    {
        [Key]                
        public Guid MedicationID { get; set; }
        public Guid PrescriptionID { get; set;}
        public string? MedicationName { get; set; }
        public int? QuantityForMorning { get; set;}
        public int? QuantityForAfternoon { get; set;}
        public string? Unit  { get; set;}
        public string? Notes { get; set;}
        public string? RouteOfAdministration { get; set;}
        public string? Warnings { get; set;}
        public DateTime? ExpiryDate { get; set;}
        public string? SideEffects { get; set;}
        public DateTime? CreatedDate { get; set;}
        public string? CreatedBy { get; set;}
        public DateTime? ModifiedDate { get; set;}
        public string?   ModifiedBy { get; set;}


  
    }
}
