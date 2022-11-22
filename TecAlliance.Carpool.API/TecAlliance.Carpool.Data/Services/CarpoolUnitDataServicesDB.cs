using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TecAlliance.Carpool.Data.Models;


namespace TecAlliance.Carpool.Data.Services
{
    public class CarpoolUnitDataServicesDB : ICarpoolUnitDataServices
    {
        public string ConnectionString { get; set; }

        public CarpoolUnitDataServicesDB()
        {
            ConnectionString = @"Data Source=localhost;Initial Catalog=carpoolapp;Integrated Security=True;";
        }
        /// <summary>
        /// checks if a carpool with the given Id exists
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public bool CheckIfCarpoolUnitExists(int id)
        {
            if (FilterCarpoolUnitListForSpecificCarpoolUnit(id) != null)
            {
                return true;
            }
            else
            {
                return false;
            }           
        }

        public int GetNewId()
        {
            int newID = 0;
            using (SqlConnection connection = new SqlConnection(ConnectionString))
            {
                string queryString = "SELECT Max(carpoolID) as carpoolID FROM CarpoolList";
                SqlCommand command = new SqlCommand(queryString, connection);
                connection.Open();
                SqlDataReader reader = command.ExecuteReader();
                try
                {
                    try
                    {
                        while (reader.Read())
                        {
                            newID = Convert.ToInt32(reader["carpoolID"]) + 1;
                        }
                    }
                    finally
                    {
                        reader.Close();
                    }
                }
                catch
                {
                    return newID;
                }
                
            }
            return newID;
        }

        /// <summary>
        /// Takes the id and searches the carpoollist for a carpool with this id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public CarpoolUnit? FilterCarpoolUnitListForSpecificCarpoolUnit(int id)
        {
            List<CarpoolUnit> carpoolUnitList = CreateCarpoolUnitList();
            foreach(CarpoolUnit carpoolUnit in carpoolUnitList)
            {
                if(carpoolUnit.Id == id)
                {
                    return carpoolUnit;
                }
            }
            return null;
        }

        /// <summary>
        /// Converts the carpool information and prints it into the file
        /// </summary>
        /// <param name="carpoolUnit"></param>
        public void PrintCarpoolUnit(CarpoolUnit carpoolUnit)
        {
            using (SqlConnection connection = new SqlConnection(ConnectionString))
            {
                string queryString = $"INSERT INTO CarpoolList (carpoolID, totalSeats, destination, origin, startTime)"+
                    $"VALUES (@Id,@SeatsCount,'@Destination','@StartLoc','@Departure')";
                SqlCommand command = new SqlCommand(queryString, connection);
                command.Parameters.Add("@Id", System.Data.SqlDbType.Int);
                using(command)
                {
                    connection.Open();
                    command.ExecuteNonQuery();
                    connection.Close();
                }
                foreach (int passenger in carpoolUnit.Passengers)
                {
                    string queryString2 = $"INSERT INTO UserCarpool (userId, carpoolID) VALUES ({passenger}, {carpoolUnit.Id})";
                    SqlCommand command2 = new SqlCommand(queryString2, connection);
                    using(command2)
                    {
                        connection.Open();
                        command2.ExecuteNonQuery();
                        connection.Close();
                    }
                }                   
            }
        }

        public CarpoolUnit GetCarpoolById(int id)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(ConnectionString))
                {
                    string queryString = $"SELECT * FROM CarpoolList WHERE carpoolID = @Id";
                    SqlCommand command = new SqlCommand(queryString, connection);
                    command.Parameters.Add("@Id", System.Data.SqlDbType.Int);
                    command.Parameters["@Id"].Value = id;
                    connection.Open();
                    SqlDataReader reader = command.ExecuteReader();
                    reader.Read();
                    int totalSeats = Convert.ToInt32(reader["totalSeats"]);
                    string origin = reader["origin"].ToString();
                    string destination = reader["destination"].ToString();
                    string startTime = reader["startTime"].ToString();
                    reader.Close();
                    string queryString2 = $"SELECT * FROM UserCarpool WHERE carpoolID = {id}";
                    List<int> passengers = new List<int>();
                    SqlCommand command2 = new SqlCommand(queryString2, connection);
                    SqlDataReader reader2 = command2.ExecuteReader();
                    try
                    {
                        while (reader2.Read())
                        {
                            int passenger = Convert.ToInt32(reader2["userID"]);
                            passengers.Add(passenger);
                        }
                    }
                    finally
                    {
                        reader2.Close();
                    }
                    return new CarpoolUnit(id, totalSeats, origin, destination, startTime, passengers);
                }
            }
            catch
            {
                return null;
            }           
        }

        /// <summary>
        /// Reads the file and returns a list with all existing carpools
        /// </summary>
        /// <returns></returns>
        public List<CarpoolUnit>? CreateCarpoolUnitList()
        {
            List<CarpoolUnit> carpoolList = new List<CarpoolUnit>();
            using (SqlConnection connection = new SqlConnection(ConnectionString))
            {
                List<int> carpoolIDs = new List<int>();
                string queryString = "SELECT [carpoolID] FROM CarpoolList";
                SqlCommand command = new SqlCommand(queryString, connection);
                connection.Open();
                SqlDataReader reader = command.ExecuteReader();
                try
                {
                    while (reader.Read())
                    {
                        carpoolIDs.Add(Convert.ToInt32(reader["carpoolID"]));
                    }
                }
                finally
                {
                    reader.Close();
                }
                foreach(int id in carpoolIDs)
                {
                    carpoolList.Add(GetCarpoolById(id));
                }
            }
            return carpoolList;
        }        

        /// <summary>
        /// Replaces old information with new information
        /// </summary>
        /// <param name="carpoolUnit"></param>
        public void UpdateCarpoolUnit(CarpoolUnit carpoolUnit)
        {
            using(SqlConnection connection = new SqlConnection(ConnectionString))
            {
                string queryString = $"UPDATE CarpoolList SET totalSeats = '{carpoolUnit.SeatsCount}', destination = '{carpoolUnit.Destination}', origin = '{carpoolUnit.StartLocation}', startTime = '{carpoolUnit.Departure}' WHERE carpoolID = {carpoolUnit.Id}";
                using (SqlCommand command = new SqlCommand(queryString, connection))
                {
                    connection.Open();
                    command.ExecuteNonQuery();
                    connection.Close();
                }
            }
        }

        public void AddPassengerToCarpool(int carpoolId, int userId)
        {
            using (SqlConnection connection = new SqlConnection(ConnectionString))
            {
                string queryString = $"INSERT INTO UserCarpool (userID, carpoolID) VALUES ({userId}, {carpoolId})";
                using (SqlCommand command = new SqlCommand(queryString, connection))
                {
                    connection.Open();
                    command.ExecuteNonQuery();
                    connection.Close();
                }
            }
        }

        public void RemovePassengerFromCarpool(int carpoolId, int userId)
        {
            using (SqlConnection connection = new SqlConnection(ConnectionString))
            {
                string queryString = $"DELETE FROM UserCarpool WHERE userID = {userId} AND carpoolID = {carpoolId}";
                using (SqlCommand command = new SqlCommand(queryString, connection))
                {
                    connection.Open();
                    command.ExecuteNonQuery();
                    connection.Close();
                }
            }
        }

        /// <summary>
        /// Deletes all information about a carpool
        /// </summary>
        /// <param name="id"></param>
        public void DeleteCarpoolUnit(int id)
        {
            using (SqlConnection connection = new SqlConnection(ConnectionString))
            {
                string queryString = $"DELETE FROM UserCarpool WHERE carpoolID={id}";
                using (SqlCommand command = new SqlCommand(queryString, connection))
                {
                    connection.Open();
                    command.ExecuteNonQuery();
                    connection.Close();
                }
                string queryString2 = $"DELETE FROM CarpoolList WHERE carpoolID={id}";
                using (SqlCommand command2 = new SqlCommand(queryString2, connection))
                {
                    connection.Open();
                    command2.ExecuteNonQuery();
                    connection.Close();
                }
            }
        }        
    }
}
