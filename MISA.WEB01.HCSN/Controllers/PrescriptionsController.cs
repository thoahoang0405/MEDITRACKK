using MEDITRACK.BaseControllers;
using MEDITRACK.BL;
using MEDITRACK.BL.BaseBL;
using MEDITRACK.BL.PrescriptionBL;
using MEDITRACK.COMMON.Entities;
using MEDITRACK.COMMON.Resource;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MySqlX.XDevAPI.Common;
using Swashbuckle.AspNetCore.Annotations;

namespace MEDITRACK.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PrescriptionsController : BasesController<PrescriptionEntity>
    {
        private IPrescriptionBL _prescriptionBL;
        public PrescriptionsController(IPrescriptionBL prescriptionBL) : base(prescriptionBL)
        {
            _prescriptionBL = prescriptionBL;

        }
        [Authorize]
        [HttpPost("Filter")]
        [SwaggerResponse(StatusCodes.Status200OK, type: typeof(PagingData))]
        [SwaggerResponse(StatusCodes.Status400BadRequest)]
        [SwaggerResponse(StatusCodes.Status500InternalServerError)]
        public IActionResult FilterFixedAsset(
        [FromQuery] string? keyword,
        [FromQuery] int? pageSize,

        [FromQuery] int pageNumber,
            [FromQuery] Guid id,
             [FromQuery] int? status

        )
        {


            var multipleResults = _prescriptionBL.FilterChoose(keyword, pageSize, pageNumber, id, status);
            if (multipleResults != null)
            {
                return StatusCode(StatusCodes.Status200OK, multipleResults);
            }
            else
            {
                return StatusCode(StatusCodes.Status400BadRequest, ErrorResource.NotFound);
            }
        }

        [SwaggerResponse(statusCode: StatusCodes.Status200OK)]
        [SwaggerResponse(statusCode: StatusCodes.Status400BadRequest)]
        [SwaggerResponse(statusCode: StatusCodes.Status500InternalServerError)]
        [Authorize]
        [HttpPost("user")]
        public IActionResult InsertPrescription([FromBody] PrescriptionEntity record)
        {
            var result = _prescriptionBL.InsertPrescription(record);
            return StatusCode(StatusCodes.Status201Created, result);
        }

        [SwaggerResponse(statusCode: StatusCodes.Status200OK)]
        [SwaggerResponse(statusCode: StatusCodes.Status400BadRequest)]
        [SwaggerResponse(statusCode: StatusCodes.Status500InternalServerError)]
        [Authorize]
        [HttpPut("user")]
        public IActionResult UpdatePrescription([FromBody] PrescriptionEntity record)
        {
            var result = _prescriptionBL.UpdatePrescription(record);
            return StatusCode(StatusCodes.Status200OK, result);
        }

        [Authorize]
        [HttpGet("medications")]
        public IActionResult GetDetailMedication(Guid id)
        {
            try

            {
                var records = _prescriptionBL.GetDetailMedication(id);

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
