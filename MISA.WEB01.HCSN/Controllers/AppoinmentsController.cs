using MEDITRACK.BaseControllers;
using MEDITRACK.BL.AppointmentBL;
using MEDITRACK.COMMON.Entities;
using MEDITRACK.COMMON.Resource;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

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
        [Authorize]
        [HttpGet("appointments")]
        public IActionResult GetAllAppointments(string? keyword , Guid id)
        {
            try

            {
                var records = _appointmentBL.GetAllAppointments(keyword,id);

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
        [HttpGet("appointmentID")]
        public IActionResult GetAppoitntById(Guid id)
        {
            try

            {
                var records = _appointmentBL.GetAppoitntById( id);

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


       
        [SwaggerResponse(statusCode: StatusCodes.Status200OK)]
        [SwaggerResponse(statusCode: StatusCodes.Status400BadRequest)]
        [SwaggerResponse(statusCode: StatusCodes.Status500InternalServerError)]
        [Authorize]
        [HttpPost("user")]
        public IActionResult InsertAppointment([FromBody] AppointmentEntity record)
        {
            var result = _appointmentBL.InsertAppointment(record);
            return StatusCode(StatusCodes.Status201Created, result);
        }

        [SwaggerResponse(statusCode: StatusCodes.Status200OK)]
        [SwaggerResponse(statusCode: StatusCodes.Status400BadRequest)]
        [SwaggerResponse(statusCode: StatusCodes.Status500InternalServerError)]
        [Authorize]
        [HttpPut("user")]
        public IActionResult UpdateAppointment([FromBody] AppointmentEntity record)
        {
            var result = _appointmentBL.UpdateAppointment(record);
            return StatusCode(StatusCodes.Status200OK, result);
        }

    }
}
