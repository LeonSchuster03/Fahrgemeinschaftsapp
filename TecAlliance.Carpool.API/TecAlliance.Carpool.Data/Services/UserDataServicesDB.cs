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
using System.Data.SqlClient;

namespace TecAlliance.Carpool.Data.Service
{
    public class UserDataServicesDB : IUserDataServices
    {
        public string ConnectionString { get; set; }

        public UserDataServicesDB()
        {
            ConnectionString = @"Data Source=localhost;Initial Catalog=carpoolapp;Integrated Security=True;";
        }

        public int GetNewId()
        {
            int newID = 0;
            using (SqlConnection connection = new SqlConnection(ConnectionString))
            {
                string queryString = "SELECT Max(userID) as userID FROM Userlist";
                SqlCommand command = new SqlCommand(queryString, connection);
                connection.Open();
                SqlDataReader reader = command.ExecuteReader();
                
                try
                {
                    while (reader.Read())
                    {
                        newID = Convert.ToInt32(reader["userID"]) + 1;
                    }
                }
                finally
                {
                    reader.Close();
                }
            }
            return newID;                
        }
        public User GetUserById(int id)
        {
            /*User user;
            try
            {
                using (SqlConnection connection = new SqlConnection(ConnectionString))
                {
                    string queryString = $"SELECT * FROM Userlist"; //WHERE userID = {id}";
                    SqlCommand command = new SqlCommand(queryString, connection);
                    connection.Open();
                    SqlDataReader reader = command.ExecuteReader();

                    if (Convert.ToInt32(reader["userID"]) == id)
                    {
                        string username = reader["username"].ToString();
                        string firstName = reader["firstName"].ToString();
                        string lastName = reader["lastName"].ToString();
                        int age = Convert.ToInt32(reader["age"]);
                        string startPlace = reader["origin"].ToString();
                        string endPlace = reader["destination"].ToString();
                        bool hasCar = false;
                        if (Convert.ToInt32(reader["canDrive"]) == 1)
                        {
                            hasCar = true;
                        }
                        user = new User(id, username, firstName, lastName, age, endPlace, startPlace, endPlace, hasCar);
                        connection.Close();
                        return user;
                    }                    
                }                            
            }
            catch
            {
                return null;
            }
            return null;*/
            List<User> userList = new List<User>();
            using (SqlConnection connection = new SqlConnection(ConnectionString))
            {
                string queryString = "SELECT * FROM Userlist";
                SqlCommand command = new SqlCommand(queryString, connection);
                connection.Open();
                SqlDataReader reader = command.ExecuteReader();
                try
                {
                    while (reader.Read())
                    {
                        int userid = Convert.ToInt32(reader["userID"]);
                        string userName = reader["username"].ToString();
                        string firstName = reader["firstName"].ToString();
                        string lastName = reader["lastName"].ToString();
                        int age = Convert.ToInt32(reader["age"]);
                        string gender = reader["gender"].ToString();
                        string origin = reader["origin"].ToString();
                        string destination = reader["destination"].ToString();
                        bool hasCar = false;
                        if (Convert.ToInt32(reader["canDrive"]) == 1)
                        {
                            hasCar = true;
                        }
                        User user = new User(userid, userName, firstName, lastName, age, gender, destination, origin, hasCar);
                        userList.Add(user);
                    }
                }
                finally
                {
                    reader.Close();
                }
                foreach(User user in userList)
                {
                    if(user.Id == id)
                    {
                        return user;
                    }
                }
            }
            return null;
        }

        /// <summary>
        /// Prints information of a user to the file
        /// </summary>
        /// <param name="user"></param>
        public void PrintUserInfo(User user)
        {
            using (SqlConnection connection = new SqlConnection(ConnectionString))
            {
                string queryString = $"INSERT INTO Userlist (userID, username, firstName, lastName, age, gender, origin, destination, canDrive)" +
                    $"VALUES ({user.Id},'{user.UserName}','{user.FirstName}','{user.LastName}',{user.Age},'{user.Gender}','{user.StartPlace}','{user.EndPlace}','{user.HasCar.ToString()}')";
                using (SqlCommand command = new SqlCommand(queryString, connection))
                {
                    connection.Open();
                    command.ExecuteNonQuery();
                    connection.Close();
                }                    
            }
        }

        /// <summary>
        /// Reads file and creates a list of all existing users
        /// </summary>
        /// <returns></returns>
        public List<User> CreateUserList()
        {
            List<User> userList = new List<User>();
            using (SqlConnection connection = new SqlConnection(ConnectionString))
            {
                string queryString = "SELECT * FROM Userlist";
                SqlCommand command = new SqlCommand(queryString, connection);
                connection.Open();
                SqlDataReader reader = command.ExecuteReader();
                try
                {
                    while (reader.Read())
                    {
                        int id = Convert.ToInt32(reader["userID"]);
                        string userName = reader["username"].ToString();
                        string firstName = reader["firstName"].ToString();
                        string lastName = reader["lastName"].ToString();
                        int age = Convert.ToInt32(reader["age"]);
                        string gender = reader["gender"].ToString();
                        string origin = reader["origin"].ToString();
                        string destination = reader["destination"].ToString();
                        bool hasCar = false;
                        if(Convert.ToInt32(reader["canDrive"]) == 1)
                        {
                            hasCar = true;
                        }
                        User user = new User(id, userName, firstName, lastName, age, gender, destination, origin, hasCar);                     
                        userList.Add(user);
                    }
                }
                finally
                {
                    reader.Close();
                }
            }
            return userList;
        }
        
        /// <summary>
        /// Replaces users' old information with the new information
        /// </summary>
        /// <param name="user"></param>
        public void UpdateUser(User user)
        {
            using (SqlConnection connection = new SqlConnection(ConnectionString))
            {
                string queryString = $"UPDATE Userlist SET username = '{user.UserName}', firstName = '{user.FirstName}', lastName = '{user.LastName}',age = '{user.Age}',gender = '{user.Gender}',origin = '{user.StartPlace}',destination = '{user.EndPlace}',canDrive = '{user.HasCar.ToString()}' WHERE userId = {user.Id}";
                using (SqlCommand command = new SqlCommand(queryString, connection))
                {
                    connection.Open();
                    command.ExecuteNonQuery();
                    connection.Close();
                }

            }
        }

        /// <summary>
        /// Removes user from the file
        /// </summary>
        /// <param name="id"></param>
        public void DeleteUser(long id)
        {
            using (SqlConnection connection = new SqlConnection(ConnectionString))
            {
                string queryString = $"DELETE FROM Userlist WHERE userID={id}";
                using (SqlCommand command = new SqlCommand(queryString, connection))
                {
                    connection.Open();
                    command.ExecuteNonQuery();
                    connection.Close();
                }
            }
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
