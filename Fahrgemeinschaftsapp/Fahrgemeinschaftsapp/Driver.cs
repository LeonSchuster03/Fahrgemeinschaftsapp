using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fahrgemeinschaftsapp  
{
    public class Driver : User
    {
        

        public int TotalSeats { get; set; }
        public int FreeSeats { get; set; }
        public DateTime TimeStart { get; set; }

        public Driver(string username, string firstname, string lastname, int age, string gender, string startplace, string endplace, bool hascar, int totalseats, int freeseats, DateTime timestart) : base(username, firstname, lastname, age, gender, startplace, endplace, hascar)
        {
            TotalSeats = totalseats;
            FreeSeats = freeseats;
            TimeStart = timestart;
        }
    }
}
