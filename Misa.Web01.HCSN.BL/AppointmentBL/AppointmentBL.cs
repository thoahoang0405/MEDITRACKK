using MEDITRACK.COMMON.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MEDITRACK.BL.AppointmentBL
{
    public class AppointmentBL:BaseBL<AppointmentEntity>, IAppointmentBL
    {

        #region contructor
        private IAppointmentDL _appointmentDL;
        public AppointmentBL(IAppointmentDL appointmentDL) : base(appointmentDL)
        {
            _appointmentDL = appointmentDL;
        }

        #endregion
    }
}
