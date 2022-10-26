using Microsoft.VisualBasic;
using System.Reflection;
using TecAlliance.Carpool.Business.Models;
using TecAlliance.Carpool.Data.Models;
using TecAlliance.Carpool.Data.Service;

namespace TecAlliance.Carpool.Business.Services
{
    public interface IUserBusinessServices
    {

        /// <summary>
        /// Converts user to userDto and activates PrintUserInfoToFile-Method
        /// </summary>
        /// <param name="userDto"></param>
        UserDto CreateUser(int id, string username, string firstName, string lastName, int age, string gender, string startPlace, string destination, bool hasCar);
        /// <summary>
        /// Creates unique Id for a user
        /// </summary>
        /// <returns></returns>
        int GetId();
        /// <summary>
        /// Checks if user exists, if not, it returns "null"
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        bool CheckIfUserExists(int id);

        /// <summary>
        /// Activates CreateUserListFromFile and searches for user with specific Id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        UserDto? GetUserById(int id);

        /// <summary>
        /// Activates CreateUserListFromFile and returns the list
        /// </summary>
        /// <returns></returns>
        List<UserDto> GetAllUsers();

        /// <summary>
        /// Returns a list with all the users having the same destination
        /// </summary>
        /// <param name="destination"></param>
        /// <returns></returns>
        List<ShortUserInfoDto> GetUsersWithSameDestination(string destination);

        /// <summary>
        /// Searches for user with given Id and returns short information about him/her
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        ShortUserInfoDto GetShortUserInfo(int id);

        /// <summary>
        /// Updates information about a user
        /// </summary>
        /// <param name="userDto"></param>
        /// <exception cref="Exception"></exception>
        UserDto UpdateUser(int id, string username, string firstName, string lastName, int age, string gender, string startPlace, string destination, bool hasCar);

        /// <summary>
        /// removes all information from a user
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        bool DeleteUser(int id);

        
    }
}