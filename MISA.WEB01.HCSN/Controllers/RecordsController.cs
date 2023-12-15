using MEDITRACK.BaseControllers;
using MEDITRACK.BL;
using MEDITRACK.BL.RecordBL;
using MEDITRACK.COMMON.Entities;
using MEDITRACK.COMMON.Resource;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace MEDITRACK.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RecordsController : BasesController<RecordsEntity>
    {
        private IRecordBL _recordBL;
        public RecordsController(IRecordBL recordBL) : base(recordBL)
        {
            _recordBL = recordBL;
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


            var multipleResults = _recordBL.FilterChoose(keyword, pageSize, pageNumber);
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
        [HttpPost("records")]
        public IActionResult InsertRecord(RecordsEntity record)
        {
            var result=_recordBL.InsertRecord(record);
            return StatusCode(StatusCodes.Status201Created, result);
        }

        [SwaggerResponse(statusCode: StatusCodes.Status200OK)]
        [SwaggerResponse(statusCode: StatusCodes.Status400BadRequest)]
        [SwaggerResponse(statusCode: StatusCodes.Status500InternalServerError)]
        [HttpPut("records")]
        public IActionResult UpdateRecord([FromBody] RecordsEntity record)
        {
            var result= _recordBL.UpdateRecord(record);
            return StatusCode(StatusCodes.Status200OK, result);
        }
    }
}
