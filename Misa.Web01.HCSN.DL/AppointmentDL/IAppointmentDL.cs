using MEDITRACK.COMMON.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MEDITRACK.BL.AppointmentBL
{
    public interface IAppointmentDL:IBaseDL<AppointmentEntity>
    {
        public IEnumerable<dynamic> GetAllAppointments(string? keyword, Guid id, int? status);
        public AppointmentEntity GetAppoitntById(Guid id);
        public AppointmentEntity InsertAppointment(AppointmentEntity record);
        public AppointmentEntity UpdateAppointment(AppointmentEntity record);
    }
}
