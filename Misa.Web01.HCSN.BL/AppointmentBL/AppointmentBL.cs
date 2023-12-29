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

        public IEnumerable<dynamic> GetAllAppointments(string? keyword ,Guid id)
        {
            return _appointmentDL.GetAllAppointments(keyword,id);
        }
        public AppointmentEntity GetAppoitntById(Guid id)
        {
            return _appointmentDL.GetAppoitntById(id);
        }
        public AppointmentEntity InsertAppointment(AppointmentEntity record)
        {
            return _appointmentDL.InsertAppointment(record);
        }
        public AppointmentEntity UpdateAppointment(AppointmentEntity record)
        {
            return (_appointmentDL.UpdateAppointment(record));
        }
    }
}
