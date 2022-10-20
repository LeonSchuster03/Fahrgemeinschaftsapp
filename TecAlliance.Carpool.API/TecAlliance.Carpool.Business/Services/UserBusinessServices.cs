using System.Reflection;
using TecAlliance.Carpool.Business.Models;
using TecAlliance.Carpool.Data.Models;
using TecAlliance.Carpool.Data.Service;

namespace TecAlliance.Carpool.Business.Services
{
    public class UserBusinessServices
    {
        UserDataServices userDataServices;
        
        public UserBusinessServices()
        {
            userDataServices = new UserDataServices();
        }
        
        /// <summary>
        /// Converts user to userDto and activates PrintUserInfoToFile-Method
        /// </summary>
        /// <param name="userDto"></param>
        public void CreateUser(UserDto userDto)
        {           
            var user = ConvertUserDtoToUser(userDto);
            //userDataServices.AddUserToCsv(user);
            userDataServices.PrintUserInfoToFile(user);
        }

        /// <summary>
        /// Creates unique Id for a user
        /// </summary>
        /// <returns></returns>
        public long GetId()
        {
            long id = 0;
            do {             
                if (!CheckIfUserExists(id))
                {
                    break;
                }
                else
                {
                    id++;
                }
            } while (true);

            return id;
        }
        /// <summary>
        /// Checks if user exists, if not, it returns "null"
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public bool CheckIfUserExists(long id)
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
        public UserDto? GetUserById(long id)
        {            
            List<User> users = userDataServices.CreateUserListFromFile();
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
            List<User> userList = userDataServices.CreateUserListFromFile();
            foreach (User user in userList)
            {
                UserDto userDto = ConvertUserToUserDto(user);
                userDtoList.Add(userDto);
            }
            return userDtoList;
        }
        
        /// <summary>
        /// Creates ShotUserInfoDto and returns it
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ShortUserInfoDto GetShortUserInfo(long id)
        {
            UserDto userDto = GetUserById(id);
            ShortUserInfoDto shortUserInfoDto = new ShortUserInfoDto(userDto.Id, userDto.FirstName, userDto.HasCar);
            return shortUserInfoDto;
        }

        /// <summary>
        /// Updates information about a user
        /// </summary>
        /// <param name="userDto"></param>
        /// <exception cref="Exception"></exception>
        public void UpdateUser(UserDto userDto)
        {
            User user = ConvertUserDtoToUser(userDto);
            if (CheckIfUserExists(user.Id))
            {               
                userDataServices.UpdateUser(user);
            }
            else
            {
                throw new Exception();
            }
        }
        
        /// <summary>
        /// removes all information from a user
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public bool DeleteUser(long id)
        {
            if (CheckIfUserExists(id))
            {
                userDataServices.DeleteUserFromFile(id);
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