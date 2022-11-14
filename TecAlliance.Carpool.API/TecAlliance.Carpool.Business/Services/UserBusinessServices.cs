using Microsoft.VisualBasic;
using System.Reflection;
using TecAlliance.Carpool.Business.Models;
using TecAlliance.Carpool.Data.Models;
using TecAlliance.Carpool.Data.Service;
using System.Linq;

namespace TecAlliance.Carpool.Business.Services
{
    public class UserBusinessServices : IUserBusinessServices
    {
        IUserDataServices userDataServices;
        
        public UserBusinessServices(IUserDataServices UserDataServices)
        {
            userDataServices = UserDataServices;
        }
        
        /// <summary>
        /// Converts user to userDto and activates PrintUserInfoToFile-Method
        /// </summary>
        /// <param name="userDto"></param>
        public UserDto CreateUser(int id, string username,string firstName, string lastName, int age, string gender, string startPlace, string destination, bool hasCar)
        {
            var userDto = new UserDto(id, username, firstName, lastName, age, gender, startPlace, destination, hasCar);
            var user = ConvertUserDtoToUser(userDto);
            //userDataServices.AddUserToCsv(user);
            userDataServices.PrintUserInfo(user);
            return userDto;
        }

        /// <summary>
        /// Creates unique Id for a user
        /// </summary>
        /// <returns></returns>
        public int GetId()
        {
            var userList = userDataServices.CreateUserList();
            var userWithHighestId = userList.OrderByDescending(user => user.Id).First();
            int id = userWithHighestId.Id + 1;
            
            //do {             
            //    if (!CheckIfUserExists(id))
            //    {
            //        break;
            //    }
            //    else
            //    {
            //        id++;
            //    }
            //} while (true);

            return id;
        }
        /// <summary>
        /// Checks if user exists, if not, it returns "null"
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public bool CheckIfUserExists(int id)
        {
            if (userDataServices.FilterUserListForSpecificUser(id) != null)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// Activates CreateUserListFromFile and searches for user with specific Id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public UserDto? GetUserById(int id)
        {            
            List<User> users = userDataServices.CreateUserList();
            foreach(User user in users)
            {
                if(user.Id == id)
                {
                    UserDto userDto = ConvertUserToUserDto(user); 
                    return userDto;
                }
            }
            return null;                        
        }

        /// <summary>
        /// Activates CreateUserListFromFile and returns the list
        /// </summary>
        /// <returns></returns>
        public List<UserDto> GetAllUsers()
        {
            List<UserDto> userDtoList = new List<UserDto>();
            List<User> userList = userDataServices.CreateUserList();
            foreach (User user in userList)
            {
                UserDto userDto = ConvertUserToUserDto(user);
                userDtoList.Add(userDto);
            }
            return userDtoList;
        }

        /// <summary>
        /// Returns a list with all the users having the same destination
        /// </summary>
        /// <param name="destination"></param>
        /// <returns></returns>
        public List<ShortUserInfoDto> GetUsersWithSameDestination(string destination)
        {
            List<ShortUserInfoDto> shortUserInfoList = new List<ShortUserInfoDto>();
            List < UserDto > userDtoList = GetAllUsers();
            foreach( UserDto userDto in userDtoList)
            {
                
                if(userDto.EndPlace == destination)
                {

                    shortUserInfoList.Add(GetShortUserInfo(userDto.Id));
                }
            }
            return shortUserInfoList;
        }
        
        /// <summary>
        /// Searches for user with given Id and returns short information about him/her
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ShortUserInfoDto? GetShortUserInfo(int id)
        {
            UserDto? userDto = GetUserById(id);
            if(userDto == null)
            {
                return null;
            }
            ShortUserInfoDto shortUserInfoDto = new ShortUserInfoDto(userDto.Id, userDto.FirstName, userDto.HasCar);
            return shortUserInfoDto;
        }

        /// <summary>
        /// Updates information about a user
        /// </summary>
        /// <param name="userDto"></param>
        /// <exception cref="Exception"></exception>
        public UserDto? UpdateUser(int id, string username, string firstName, string lastName, int age, string gender, string startPlace, string destination, bool hasCar)
        {
            var userDto = new UserDto(id, username, firstName, lastName, age, gender, startPlace, destination, hasCar);
            User user = ConvertUserDtoToUser(userDto);
            if (CheckIfUserExists(user.Id))
            {               
                userDataServices.UpdateUser(user);
            }
            else
            {
                return null;
            }
            return userDto;
        }
        
        /// <summary>
        /// removes all information from a user
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public bool DeleteUser(int id)
        {
            if (CheckIfUserExists(id))
            {
                userDataServices.DeleteUser(id);
                return true;
            }
            else
            {
                return false;
            }            
        }

        /// <summary>
        /// Converts User object to UserDto object
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        public UserDto ConvertUserToUserDto(User user)
        {
            var userDto = new UserDto(user.Id, user.UserName, user.FirstName, user.LastName, user.Age, user.Gender, user.StartPlace, user.EndPlace, user.HasCar);
            return userDto;
        }

        /// <summary>
        /// Converts UserDto object to User object
        /// </summary>
        /// <param name="userDto"></param>
        /// <returns></returns>
        public User ConvertUserDtoToUser(UserDto userDto)
        {
            var user = new User(userDto.Id, userDto.UserName, userDto.FirstName, userDto.LastName, userDto.Age, userDto.Gender, userDto.StartPlace, userDto.EndPlace, userDto.HasCar);
            return user;
        }
    }
}