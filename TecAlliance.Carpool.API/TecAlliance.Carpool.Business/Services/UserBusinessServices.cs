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
            var user = new User(userDto.Id, userDto.UserName, userDto.FirstName, userDto.LastName, userDto.Age, userDto.Gender, userDto.StartPlace, userDto.EndPlace, userDto.HasCar);
            userDataServices.AddUserToCsv(user);
        }

        public long GetId()
        {
            long id = 0;
            do {
                FileInfo fi = new FileInfo($"C:\\010Projects\\019 Fahrgemeinschaft\\Fahrgemeinschaftsapp\\Userlist\\{id}.csv");
                
                if (fi.Exists)
                {
                    id++;
                }
                else
                {
                    using (FileStream fs = File.Create($"C:\\010Projects\\019 Fahrgemeinschaft\\Fahrgemeinschaftsapp\\Userlist\\{id}.csv"));
                    break;
                }
            } while (true);

            return id;
        }

        public UserDto GetUserById(long id)
        {
            UserDto userdto;
            List<User> users = userDataServices.GetAllUsers();
            foreach(User user in users)
            {
                if(user.Id == id)
                {
                    userdto = new UserDto(user.Id, user.UserName, user.FirstName, user.LastName, user.Age, user.Gender, user.StartPlace, user.EndPlace, user.HasCar);
                    return userdto;
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
                UserDto userdto = new UserDto(user.Id, user.UserName, user.FirstName, user.LastName, user.Age, user.Gender, user.StartPlace, user.EndPlace, user.HasCar);
                userDtoList.Add(userdto);
            }
            return userDtoList;
        }
        
        public void DeleteUser(long id)
        {
            userDataServices.DeleteUserFromFile(id);            
        }
    }
}