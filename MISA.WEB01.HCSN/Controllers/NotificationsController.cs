using MEDITRACK.BaseControllers;
using MEDITRACK.BL;
using MEDITRACK.BL.BaseBL;
using MEDITRACK.BL.NoticeBL;
using MEDITRACK.COMMON.Entities;
using MEDITRACK.COMMON.Resource;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace MEDITRACK.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class NotificationsController : BasesController<NotificationsEntity>
    {
        private INotificationBL _notificationBL;
        public NotificationsController(INotificationBL notification) : base(notification)
        {
            _notificationBL = notification;
        }
        [Authorize]
        [HttpGet("appointmentDay")]
        public IActionResult GetAppointmentDay([FromQuery] Guid id)
        {
            try

            {
                var records = _notificationBL.GetAppointmentDay(id);

                if (records != null)
                {
                    return StatusCode(StatusCodes.Status200OK, records);
                }
                else
                {
                    return StatusCode(StatusCodes.Status404NotFound, ErrorResource.NotFound);
                }

            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ErrorResource.ServerException);
            }
        }
        [Authorize]
        [HttpGet("presDay")]
        public IActionResult GetPrescriptionDay([FromQuery] Guid id)
        {
            try

            {
                var records = _notificationBL.GetPrescriptionDay(id);

                if (records != null)
                {
                    return StatusCode(StatusCodes.Status200OK, records);
                }
                else
                {
                    return StatusCode(StatusCodes.Status404NotFound, ErrorResource.NotFound);
                }

            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ErrorResource.ServerException);
            }
        }
    }
}
