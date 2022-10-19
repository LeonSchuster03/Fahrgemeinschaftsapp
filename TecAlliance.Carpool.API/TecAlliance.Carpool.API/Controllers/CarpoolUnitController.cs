using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TecAlliance.Carpool.Business.Models;
using TecAlliance.Carpool.Business.Services;

namespace TecAlliance.Carpool.API.Controllers
{
    [Route("api/CarpoolUnitController")]
    [ApiController]
    public class CarpoolUnitController : ControllerBase
    {
        CarpoolUnitBusinessServices businessServices;
        public CarpoolUnitController()
        {
            businessServices = new CarpoolUnitBusinessServices();
        }

        [HttpPost]
        public ActionResult<CarpoolUnitDto> PostCarpoolUnitDto(CarpoolUnitDto carpoolDto)
        {
            
            carpoolDto.Id = businessServices.GetId();
            businessServices.CreateCarpoolUnit(carpoolDto);

            return Created($"api/CarpoolUnitController/{carpoolDto.Id}", carpoolDto);
        }

        [HttpGet("{id}")]
        public ActionResult<CarpoolUnitDto> GetCarpoolUnitById(long id)
        {
            if(businessServices.GetCarpoolUnitById(id) == null)
            {
                return StatusCode(404);
            }
            else
            {
                return businessServices.GetCarpoolUnitById(id);
            }
        }

        [HttpGet]
        public ActionResult<List<CarpoolUnitDto>> GetAllCarpoolUnits()
        {
            return businessServices.GetAllCarpoolUnits();
        }
    }
}
