using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TecAlliance.Carpool.Business.Services;
using TecAlliance.Carpool.Business.Models;

namespace TecAlliance.Carpool.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        UserBusinessServices businessServices;
        public UserController()
        {
            businessServices = new UserBusinessServices();
        }

        [HttpPost]
        public ActionResult<UserDto> PostUserDto(UserDto userDto)
        {
            userDto.Id = businessServices.GetId();
            businessServices.CreateUser(userDto);
            
            return Created($"api/User/{userDto.Id}", userDto); 
        }

        [HttpGet("{id}")]
        public ActionResult<UserDto> GetUserById(long id)
        {
            return businessServices.GetUserById(id);
        }

        [HttpGet]
        public ActionResult<List<UserDto>> GetAllUsers()
        {
            return businessServices.GetAllUsers();
        }
    }
}
