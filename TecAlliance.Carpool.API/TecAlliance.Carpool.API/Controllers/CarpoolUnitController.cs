using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Runtime.InteropServices;
using TecAlliance.Carpool.Business.Models;
using TecAlliance.Carpool.Business.Services;

namespace TecAlliance.Carpool.API.Controllers
{
    [Route("api/CarpoolUnitController")]
    [ApiController]
    public class CarpoolUnitController : ControllerBase
    {
        ICarpoolUnitBusinessServices businessServices;
        public CarpoolUnitController(ICarpoolUnitBusinessServices carpoolUnitBusiness)
        {
            businessServices = carpoolUnitBusiness;
        }

        /// <summary>
        /// Creates a Carpool
        /// </summary>
        /// <param name="carpoolDto"></param>
        /// <returns>
        /// Returns a newly created carpool
        /// </returns>
        [HttpPost]
        public ActionResult<CarpoolUnitDto> PostCarpoolUnitDto(int seatsCount, string destination, string startLocation, string departure, List<int> passengers)
        {            
            var carpoolUnitDto = businessServices.CreateCarpoolUnit(businessServices.GetId(),  seatsCount,  destination,  startLocation,  departure,passengers);

            return Created($"api/CarpoolUnitController/{carpoolUnitDto.Id}", carpoolUnitDto);
        }

        /// <summary>
        /// returns carpool specific ID
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{id}")]
        public ActionResult<CarpoolUnitDto>? GetCarpoolUnitById(int id)
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

        /// <summary>
        /// Returns all existing carpools
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult<List<CarpoolUnitDto>>? GetAllCarpoolUnits()
        {
            return businessServices.GetAllCarpoolUnits();
        }

        /// <summary>
        /// Returns carpool with specific Id 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("GetUserInCarpool/{id}")]
        public ActionResult<List<int>>? GetUsersInCarpool(int id)
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

        /// <summary>
        /// Updates information of a carpool
        /// </summary>
        /// <param name="carpoolUnitDto"></param>
        /// <returns></returns>
        [HttpPut("{id}")]
        public ActionResult<CarpoolUnitDto> UpdateCarpoolUnit(CarpoolUnitDto carpoolUnitDto)
        {
            businessServices.UpdateCarpoolUnit(carpoolUnitDto);
            return carpoolUnitDto;
        }

        /// <summary>
        /// Removes all stored informatin about a carpool
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete("{id}")]
        public ActionResult<CarpoolUnitDto> DeleteCarpoolUnit(int id)
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

        /// <summary>
        /// User can join a carpool by entering the carpool Id
        /// </summary>
        /// <param name="cpId"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        [HttpPut("join")]
        public ActionResult<CarpoolUnitDto> JoinCarpoolUnit(int cpId, int userId)
        {
            if(businessServices.JoinCarpoolUnit(cpId, userId ) != null)
            {
                return businessServices.JoinCarpoolUnit(cpId, userId);
            }
            else
            {
                return StatusCode(404);
            }
        }

        /// <summary>
        /// User can leave a carpool by entering the carpool Id
        /// </summary>
        /// <param name="cpId"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        [HttpPut("leave")]
        public ActionResult<CarpoolUnitDto> LeaveCarpoolUnit(int cpId, int userId)
        {
            if(businessServices.LeaveCarpoolUnit(cpId, userId) != null)
            {
                return businessServices.LeaveCarpoolUnit(cpId, userId);
            }
            else
            {
                return StatusCode(404);
            }
        }        
    }
}
