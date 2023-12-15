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
    [Table("notifications")]
    public class NotificationsEntity
    {
        [Key]
        public Guid NoticeID { get; set; }
        public Guid UserID { get; set; }
        public string NoticeName { get; set; }
        public DateTime NoticeDate { get; set;}
        public int NoticeStatus { get; set;}
        public int NoticeType { get; set;}
        public int NoticeOfUser { get; set;}
        public DateTime? CreatedDate { get; set; }
        public string? CreatedBy { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public string? ModifiedBy { get; set; }
    }
}
