using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TecAlliance.Carpool.Business.Services;
using TecAlliance.Carpool.Business.Models;
using System.Data.Common;

namespace TecAlliance.Carpool.API.Controllers
{
    [Route("api/UserController")]
    [ApiController]
    public class UserController : ControllerBase
    {
        UserBusinessServices businessServices;
        public UserController()
        {
            businessServices = new UserBusinessServices();
        }

        /// <summary>
        /// Creates a User
        /// </summary>
        /// <param name="userDto"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult<UserDto> PostUserDto(UserDto userDto)
        {
            userDto.Id = businessServices.GetId();
            businessServices.CreateUser(userDto);
            
            return Created($"api/UserController/{userDto.Id}", userDto); 
        }

        /// <summary>
        /// returns information of a user with specifi ID
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{id}")]
        public ActionResult<UserDto> GetUserById(long id)
        {
            UserDto userDto = businessServices.GetUserById(id);
            if(userDto != null)
            {
                return userDto;
            }
            else
            {
                return StatusCode(404);
            }
        }

        /// <summary>
        /// returns information from all existing users
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult<List<UserDto>> GetAllUsers()
        {
            return businessServices.GetAllUsers();
        }

        /// <summary>
        /// Updates information about specific user
        /// </summary>
        /// <param name="userDto"></param>
        /// <returns></returns>
        [HttpPut("{id}")]
        public ActionResult<UserDto> UpdateUser(UserDto userDto)
        {
            businessServices.UpdateUser(userDto);
            return userDto;
        }

        /// <summary>
        /// Returns Id, Name and HasCar of a user
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("GetShortInfoOfUser/{id}")]
        public ActionResult<ShortUserInfoDto> GetShortUserInfo(long id)
        {
            return businessServices.GetShortUserInfo(id);
        }

        /// <summary>
        /// Removes all stored information about a user
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
       [HttpDelete("{id}")]
        public ActionResult<UserDto> DeleteUser(long id)
        {           
            if(businessServices.DeleteUser(id)) 
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
