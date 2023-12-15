using MEDITRACK.BaseControllers;
using MEDITRACK.BL.AppointmentBL;
using MEDITRACK.COMMON.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace MEDITRACK.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AppoinmentsController : BasesController<AppointmentEntity>
    {
        #region Field
        private IAppointmentBL _appointmentBL;
        #endregion

        #region Contructor
        public AppoinmentsController(IAppointmentBL appointmentBL) : base(appointmentBL)
        {
            _appointmentBL = appointmentBL;
        }
        #endregion
    }
}
