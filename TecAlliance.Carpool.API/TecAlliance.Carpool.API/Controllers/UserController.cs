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
        public ActionResult<UserDto> GetUserById(int id)
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
        /// By entering a destination the program returns a short information about all users with the same destination
        /// </summary>
        /// <param name="destination"></param>
        /// <returns></returns>
        [HttpGet("destination")]
        public ActionResult<List<ShortUserInfoDto>> GetUsersWithSameDestination(string destination)
        {
            if(businessServices.GetUsersWithSameDestination(destination) != null)
            {
                return businessServices.GetUsersWithSameDestination(destination);
            }
            else
            {
                return StatusCode(404);
            }           
        }

        /// <summary>
        /// By entering Ids seperated by a comma, it returns a list with information of the users with that Id
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        [HttpGet("userlist")]
        public ActionResult<List<ShortUserInfoDto>> GetUsersWithTheIds(List<int> ids)
        {
            return businessServices.GetUsersWithIds(ids);
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
        public ActionResult<ShortUserInfoDto> GetShortUserInfo(int id)
        {
            return businessServices.GetShortUserInfo(id);
        }

        /// <summary>
        /// Removes all stored information about a user
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
       [HttpDelete("{id}")]
        public ActionResult<UserDto> DeleteUser(int id)
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
