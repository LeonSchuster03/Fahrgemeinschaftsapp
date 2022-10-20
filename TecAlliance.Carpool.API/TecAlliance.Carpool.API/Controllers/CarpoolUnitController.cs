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
            if(businessServices.GetCarpoolUnitById(id) != null)
            {
                return businessServices.GetCarpoolUnitById(id);   
            }
            else
            {
                return StatusCode(404);
            }
        }

        [HttpGet]
        public ActionResult<List<CarpoolUnitDto>> GetAllCarpoolUnits()
        {
            return businessServices.GetAllCarpoolUnits();
        }

        [HttpGet("GetUserInCarpool/{id}")]
        public ActionResult<List<long>> GetUsersInCarpool(long id)
        {
            if(businessServices.GetUsersInCarpool(id) != null)
            {
                return businessServices.GetUsersInCarpool(id);
            }
            else
            {
                return null;
            }
        }

        [HttpPut("{id}")]
        public ActionResult<CarpoolUnitDto> UpdateCarpoolUnit(CarpoolUnitDto carpoolUnitDto)
        {
            businessServices.UpdateCarpoolUnit(carpoolUnitDto);
            return carpoolUnitDto;
        }

        [HttpDelete("{id}")]
        public ActionResult<CarpoolUnitDto> DeleteCarpoolUnit(long id)
        {
            if (businessServices.DeleteCarpoolUnit(id))
            {
                return StatusCode(204);
            }
            else
            {
                return StatusCode(404);
            }
        }
        
    }
}
