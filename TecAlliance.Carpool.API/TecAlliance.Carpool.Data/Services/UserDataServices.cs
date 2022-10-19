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
    public class UserDataServices
    {
        public User? BuildUserFromLine(string line)
        {
            if(line != null)
            {
                string[] info = line.Split(";");
                User user = new User(long.Parse(info[0]), info[1], info[2], info[3], Convert.ToInt32(info[4]), info[5], info[6], info[7], Convert.ToBoolean(info[8]));
                return user;
            }
            else
            {
                return null;
            }
        }
        public List<User> CreateUserListFromFile()
        {
            List<User> userList = new List<User>();
            string[] fileText = File.ReadAllLines($"C:\\010Projects\\019 Fahrgemeinschaft\\Fahrgemeinschaftsapp\\Userlist.csv");

            //Builds User
            foreach(string userText in fileText)
            {
                User user = BuildUserFromLine(userText);
                userList.Add(user);
            }
            return userList;
        }
        
        public void UpdateUser(User user)
        {
            DeleteUserFromFile(user.Id);
            PrintUserInfoToFile(user);
        }

        public void DeleteUserFromFile(long id)
        {
            string[] lines = File.ReadAllLines($"C:\\010Projects\\019 Fahrgemeinschaft\\Fahrgemeinschaftsapp\\Userlist.csv");
            List<string> linesToWrite = new List<string>();
            
            foreach(string s in lines)
            {
                if (!s.Contains($"{id};"))
                {
                    linesToWrite.Add(s);
                }
            }
            File.WriteAllLines($"C:\\010Projects\\019 Fahrgemeinschaft\\Fahrgemeinschaftsapp\\Userlist.csv", linesToWrite);
        }

        public void PrintUserInfoToFile(User user)
        {
                var newLine = $"{user.Id};{user.UserName};{user.FirstName};{user.LastName};{user.Age};{user.Gender};{user.StartPlace};{user.EndPlace};{user.HasCar}\n";
                File.AppendAllText($"C:\\010Projects\\019 Fahrgemeinschaft\\Fahrgemeinschaftsapp\\Userlist.csv", newLine);
        }

        public User? FilterUserListForSpecificUser(long id)
        {
            List<User> userList = CreateUserListFromFile();
            foreach(User user in userList)
            {
                if (user.Id == id)
                {
                    return user;
                }
            }
            return null;
         
        }

        
    }
}
