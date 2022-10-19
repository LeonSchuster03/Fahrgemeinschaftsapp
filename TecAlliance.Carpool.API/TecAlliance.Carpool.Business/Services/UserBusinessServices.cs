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
            userDataServices.CreateUser(user);
        }

        public long GetId()
        {
            long id = 0;
            do {             
                if (!userDataServices.CheckIfUserExists(id))
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

        public UserDto GetUserById(long id)
        {            
            List<User> users = userDataServices.GetAllUsers();
            foreach(User user in users)
            {
                if(user.Id == id)
                {
                    UserDto userDto = ConvertUserToUserDto(user);
                    return userDto;
                }
            }
            throw new Exception("User not found");                        
        }

        public List<UserDto> GetAllUsers()
        {
            List<UserDto> userDtoList = new List<UserDto>();
            List<User> userList = userDataServices.GetAllUsers();
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
            if (userDataServices.CheckIfUserExists(user.Id))
            {               
                userDataServices.PrintUserInfoToFile(user);
            }
            else
            {
                throw new Exception("User not found");
            }
        }
        
        public void DeleteUser(long id)
        {
            if (userDataServices.CheckIfUserExists(id))
            {
                userDataServices.DeleteUserFromFile(id);
            }
            else
            {
                throw new Exception("User not found");
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