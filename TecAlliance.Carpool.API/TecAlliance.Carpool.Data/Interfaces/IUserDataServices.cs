using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using TecAlliance.Carpool.Data.Models;

namespace TecAlliance.Carpool.Data.Service
{
    public interface IUserDataServices
    {
        /// <summary>
        /// adds a user to the storage type
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        User GetUserById(int id);
        /// <summary>
        /// Creates a list with all existing User objects
        /// </summary>
        /// <returns></returns>
        List<User> CreateUserList();

        /// <summary>
        /// Replaces a users' old information with the new information
        /// </summary>
        /// <param name="user"></param>
        void UpdateUser(User user);

        /// <summary>
        /// removes all stored information about the user with given Id
        /// </summary>
        /// <param name="id"></param>
        void DeleteUser(long id);

        /// <summary>
        /// Adds information to the storage unit
        /// </summary>
        /// <param name="user"></param>
        void PrintUserInfo(User user);

        /// <summary>
        /// tbd
        /// </summary>
        /// <returns></returns>
        int GetNewId();

        /// <summary>
        /// filters user with specific Id out of the Userlist
        /// </summary>
        /// <param name="id"></param>
        /// <returns>User object if user exists, otherwise it returns null</returns>
        User? FilterUserListForSpecificUser(long id);

    }
}
