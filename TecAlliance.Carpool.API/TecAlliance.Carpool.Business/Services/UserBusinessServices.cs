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
    }
}