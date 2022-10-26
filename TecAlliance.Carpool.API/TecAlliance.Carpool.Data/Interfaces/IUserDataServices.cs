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
        /// Reads file and creates a list of all existing users
        /// </summary>
        /// <returns></returns>
        List<User> CreateUserListFromFile();


        /// <summary>
        /// Replaces users' old information with the new information
        /// </summary>
        /// <param name="user"></param>
        void UpdateUser(User user);


        /// <summary>
        /// Removes user from the file
        /// </summary>
        /// <param name="id"></param>
        void DeleteUserFromFile(long id);


        /// <summary>
        /// Prints information of a user to the file
        /// </summary>
        /// <param name="user"></param>
        void PrintUserInfoToFile(User user);


        /// <summary>
        /// filters user with specific Id out of the Userlist
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        User FilterUserListForSpecificUser(long id);

    }
}
