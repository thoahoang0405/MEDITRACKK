using MEDITRACK.BL.BaseBL;
using MEDITRACK.COMMON.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MEDITRACK.DL.NoticeDL
{
    public class NotificationDL: BaseDL<NotificationsEntity>, INotificationDL
    {
        #region Field
        readonly string _connectionDB = DatabaseContext.ConnectionString;

        #endregion
    }
}
