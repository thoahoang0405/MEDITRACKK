using MEDITRACK.BaseControllers;
using MEDITRACK.BL;
using MEDITRACK.BL.PrescriptionBL;
using MEDITRACK.COMMON.Entities;
using MEDITRACK.COMMON.Resource;
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
        [HttpPost("Filter")]
        [SwaggerResponse(StatusCodes.Status200OK, type: typeof(PagingData))]
        [SwaggerResponse(StatusCodes.Status400BadRequest)]
        [SwaggerResponse(StatusCodes.Status500InternalServerError)]
        public IActionResult FilterFixedAsset(
    [FromQuery] string? keyword,
    [FromQuery] int? pageSize,
    
    [FromQuery] int pageNumber
   
    )
        {


            var multipleResults = _prescriptionBL.FilterChoose(keyword, pageSize, pageNumber);
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
        [HttpPost("user")]
        public IActionResult InsertPrescription([FromBody] PrescriptionEntity record)
        {
            var result= _prescriptionBL.InsertPrescription(record);
            return   StatusCode(StatusCodes.Status201Created, result);
        }

        [SwaggerResponse(statusCode: StatusCodes.Status200OK)]
        [SwaggerResponse(statusCode: StatusCodes.Status400BadRequest)]
        [SwaggerResponse(statusCode: StatusCodes.Status500InternalServerError)]
        [HttpPut("user")]
        public IActionResult UpdatePrescription([FromBody]  PrescriptionEntity record)
        {
            var result = _prescriptionBL.UpdatePrescription(record);
            return StatusCode(StatusCodes.Status200OK, result);
        }
    }
}
