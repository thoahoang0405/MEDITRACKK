using MEDITRACK.COMMON.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MEDITRACK.BL.NoticeBL
{
    public interface INotificationBL : IBaseBL<NotificationsEntity>
    {
        public IEnumerable<dynamic> GetAppointmentDay(Guid id);
        public IEnumerable<dynamic> GetPrescriptionDay(Guid id);
    }
}
