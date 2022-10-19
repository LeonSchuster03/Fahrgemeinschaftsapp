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
        public ActionResult<CarpoolUnitDto> PosCarpoolUnitDto(CarpoolUnitDto carpoolDto)
        {
            carpoolDto.Id = businessServices.GetId();
            businessServices.CreateCarpoolUnit(carpoolDto);

            return Created($"api/CarpoolUnitController/{carpoolDto.Id}", carpoolDto);
        }
    }
}
