using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fahrgemeinschaftsapp
{
    public class Carpool
    {
        public int Id { get; set; }
        public int PassengerCount { get; set; }
        public string Destination { get; set; }
        public string StartLocation { get; set; }
        public DateTime Departure { get; set; }

        public Carpool(int id, int passengercount, string destination,string startlocation, DateTime departure)
        {
            Id = id;
            PassengerCount = passengercount;
            Destination = destination;
            StartLocation = startlocation;
            Departure = departure;
        }



    }
}
