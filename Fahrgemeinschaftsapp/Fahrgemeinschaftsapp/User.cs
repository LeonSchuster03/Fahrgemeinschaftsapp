using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fahrgemeinschaftsapp
{
    public class User
    {
            public string Username { get; set; }
            public string FirstName { get; set; }
            public string LastName { get; set; }
            public int Age { get; set; }
            public string Gender { get; set; }
            public string StartPlace { get; set; }
            public string EndPlace { get; set; }
            public bool HasCar { get; set; }

        public User(string username, string   firstname, string lastname, int age, string gender, string startplace, string endplace, bool hascar)
        {
            Username = username;
            FirstName = firstname;
            LastName = lastname;
            Age = age;
            Gender = gender;
            StartPlace = startplace;
            EndPlace = endplace;
            HasCar = hascar;
        }
    }
}
