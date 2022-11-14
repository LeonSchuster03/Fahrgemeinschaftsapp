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
    public class UserDataServices : IUserDataServices
    {
        public string Path { get; set; }

        public UserDataServices()
        {
            Path = Directory.GetCurrentDirectory() + "\\..\\TecAlliance.Carpool.Data\\Userlist.csv";
        }

        /// <summary>
        /// Splits string, creates User object and returns it
        /// </summary>
        /// <param name="line"></param>
        /// <returns></returns>        
        public User? BuildUser(string line)
        {
            if(line != null)
            {
                string[] info = line.Split(";");
                User user = new User(int.Parse(info[0]), info[1], info[2], info[3], Convert.ToInt32(info[4]), info[5], info[6], info[7], Convert.ToBoolean(info[8]));
                return user;
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// Reads file and creates a list of all existing users
        /// </summary>
        /// <returns></returns>
        public List<User> CreateUserList()
        {
            List<User> userList = new List<User>();
            string[] fileText = File.ReadAllLines(Path);
            foreach (string userText in fileText)
            {
                User? user = this.BuildUser(userText);
                userList.Add(user);
            }
            return userList;
        }
        
        /// <summary>
        /// Replaces users' old information with the new information
        /// </summary>
        /// <param name="user"></param>
        public void UpdateUser(User user)
        {
            this.DeleteUser(user.Id);
            this.PrintUserInfo(user);
        }

        /// <summary>
        /// Removes user from the file
        /// </summary>
        /// <param name="id"></param>
        public void DeleteUser(long id)
        {
            string[] lines = File.ReadAllLines(Path);
            List<string> linesToWrite = new List<string>();
            
            foreach(string s in lines)
            {
                if (!s.Contains($"{id};"))
                {
                    linesToWrite.Add(s);
                }
            }
            File.WriteAllLines(Path, linesToWrite);
        }

        /// <summary>
        /// Prints information of a user to the file
        /// </summary>
        /// <param name="user"></param>
        public void PrintUserInfo(User user)
        {
            string path = Directory.GetCurrentDirectory();
            var newLine = $"{user.Id};{user.UserName};{user.FirstName};{user.LastName};{user.Age};{user.Gender};{user.StartPlace};{user.EndPlace};{user.HasCar}\n";
            File.AppendAllText(Path, newLine);
            
        }

        /// <summary>
        /// filters user with specific Id out of the Userlist
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public User? FilterUserListForSpecificUser(long id)
        {
            List<User> userList = CreateUserList();
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
