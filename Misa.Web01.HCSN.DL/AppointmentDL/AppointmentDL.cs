using MEDITRACK.BL.AppointmentBL;
using MEDITRACK.BL.BaseBL;
using MEDITRACK.COMMON.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MEDITRACK.DL.AppointmentDL
{
    public class AppointmentDL:BaseDL<AppointmentEntity>, IAppointmentDL
    {
        #region Field
        readonly string _connectionDB = DatabaseContext.ConnectionString;

        #endregion
    }
}
