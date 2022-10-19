using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using TecAlliance.Carpool.Data.Models;

namespace TecAlliance.Carpool.Data.Service
{
    public class UserDataServices
    {
        public void CreateUser(User user)
        {
            using (FileStream fs = File.Create($"C:\\010Projects\\019 Fahrgemeinschaft\\Fahrgemeinschaftsapp\\Userlist\\{user.Id}.csv")) ;
            PrintUserInfoToFile(user);
        }

        public List<User> GetAllUsers()
        {
            List<User> userList = new List<User>();
            foreach(string file in Directory.EnumerateFiles($"C:\\010Projects\\019 Fahrgemeinschaft\\Fahrgemeinschaftsapp\\Userlist\\"))
            {
                userList.Add(BuildUserFromFile(file));               
            }
            return userList;
        }

        public User BuildUserFromFile(string path)
        {
            var text = File.ReadAllText(path).Replace("\r\n", string.Empty);
            string[] info = text.Split(';');
            User user = new User(long.Parse(info[0]), info[1], info[2], info[3], Convert.ToInt32(info[4]), info[5], info[6], info[7], Convert.ToBoolean(info[8]));
            return user;            
        }

        public void UpdateUser(User user)
        {
            PrintUserInfoToFile(user);
        }

        public void DeleteUserFromFile(long id)
        {
            foreach(string file in Directory.EnumerateFiles($"C:\\010Projects\\019 Fahrgemeinschaft\\Fahrgemeinschaftsapp\\Userlist\\"))
            {
                User user = BuildUserFromFile(file);
                if(user.Id == id)
                {
                    File.Delete(file);
                }
            }
        }

        public void PrintUserInfoToFile(User user)
        {
            using (StreamWriter writer = new StreamWriter($"C:\\010Projects\\019 Fahrgemeinschaft\\Fahrgemeinschaftsapp\\Userlist\\{user.Id}.csv"))
            {
                var newLine = $"{user.Id};{user.UserName};{user.FirstName};{user.LastName};{user.Age};{user.Gender};{user.StartPlace};{user.EndPlace};{user.HasCar}";
                writer.WriteLine(newLine);
            }
        }
    }
}
