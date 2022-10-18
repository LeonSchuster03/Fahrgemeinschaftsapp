using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using TecAlliance.Carpool.Data.Models;

namespace TecAlliance.Carpool.Data.Service
{
    public class UserDataServices
    {
        public void AddUserToCsv(User user)
        {
            using (StreamWriter writer = new StreamWriter($"C:\\010Projects\\019 Fahrgemeinschaft\\Fahrgemeinschaftsapp\\Userlist\\{user.Id}.csv"))
            {
                var newLine = $"{user.Id};{user.UserName};{user.FirstName};{user.LastName};{user.Age};{user.Gender};{user.StartPlace};{user.EndPlace};{user.HasCar}";
                writer.WriteLine(newLine);
            }
        }
    }
}
