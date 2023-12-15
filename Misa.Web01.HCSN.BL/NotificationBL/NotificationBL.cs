using MEDITRACK.COMMON.Entities;
using MEDITRACK.DL.NoticeDL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MEDITRACK.BL.NoticeBL
{
    public class NotificationBL:BaseBL<NotificationsEntity>, INotificationBL
    {
        #region contructor
        private INotificationDL _NotificationDL;
        public NotificationBL(INotificationDL NotificationDL) : base(NotificationDL)
        {
            _NotificationDL = NotificationDL;
        }

        #endregion
    }
}
