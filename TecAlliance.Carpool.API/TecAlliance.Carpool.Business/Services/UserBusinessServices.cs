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
      
        public void CreateUser(UserDto userDto)
        {
            
            var user = ConvertUserDtoToUser(userDto);
            //userDataServices.AddUserToCsv(user);
            userDataServices.PrintUserInfoToFile(user);
        }

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

        public UserDto ConvertUserToUserDto(User user)
        {
            var userDto = new UserDto(user.Id, user.UserName, user.FirstName, user.LastName, user.Age, user.Gender, user.StartPlace, user.EndPlace, user.HasCar);
            return userDto;
        }

        public User ConvertUserDtoToUser(UserDto userDto)
        {
            var user = new User(userDto.Id, userDto.UserName, userDto.FirstName, userDto.LastName, userDto.Age, userDto.Gender, userDto.StartPlace, userDto.EndPlace, userDto.HasCar);
            return user;
        }
    }
}