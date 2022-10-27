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
        IUserBusinessServices businessServices;
        public UserController(IUserBusinessServices userBusinessServices)
        {
            businessServices = userBusinessServices;
        }

        /// <summary>
        /// Creates a User
        /// </summary>
        /// <param name="userDto"></param>
        /// <returns></returns>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public ActionResult<UserDto> PostUserDto(string username, string firstName, string lastName, int age, string gender, string startPlace, string destination, bool hasCar)
        {

            var userDto = businessServices.CreateUser(businessServices.GetId(),username,firstName,lastName,age,gender,startPlace,destination, hasCar) ;
            
            return Created($"api/UserController/{userDto.Id}", userDto); 
        }

        /// <summary>
        /// returns information of a user with specifi ID
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status302Found)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public ActionResult<UserDto> GetUserById(int id)
        {
            UserDto? userDto = businessServices.GetUserById(id);
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
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status302Found)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
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
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status302Found)]
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
        /// Updates information about specific user
        /// </summary>
        /// <param name="userDto"></param>
        /// <returns></returns>
        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status202Accepted)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public ActionResult<UserDto> UpdateUser(int id, string username, string firstName, string lastName, int age, string gender, string startPlace, string destination, bool hasCar)
        {
            var userDto = businessServices.UpdateUser( id,  username,  firstName,  lastName,  age,  gender,  startPlace,  destination,  hasCar);
            return userDto;
        }

        /// <summary>
        /// Returns Id, Name and HasCar of a user
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("GetShortInfoOfUser/{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
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
        [ProducesResponseType(StatusCodes.Status205ResetContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
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
