﻿using MEDITRACK.BL;
using MEDITRACK.COMMON.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MEDITRACK.DL.NoticeDL
{
    public interface INotificationDL:IBaseDL<NotificationsEntity>
    {
        public IEnumerable<dynamic> GetAppointmentDay(Guid id);
        public IEnumerable<dynamic> GetPrescriptionDay(Guid id);
    }
}
