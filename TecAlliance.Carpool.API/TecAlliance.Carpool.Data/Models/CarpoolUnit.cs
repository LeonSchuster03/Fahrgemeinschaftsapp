using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace TecAlliance.Carpool.Data.Models
{
    public class CarpoolUnit
    {
        public int Id { get; set; } //sets  and returns the Id of the CarpoolUnit
        public int SeatsCount { get; set; } //sets and returns the Number of seats in the CarpoolUnit
        public string Destination { get; set; } //sets and returns the destination of the CarpoolUnit
        public string StartLocation { get; set; } //sets and returns the startlocation of the CarpoolUnit
        public string Departure { get; set; } //sets and returns the time the carpool wants to start
        public List<int> Passengers { get; set; } //sets and returns a list of UserIds of the passengers

        public CarpoolUnit(int id, int seatscount, string destination, string startlocation, string departure, List<int> passengers)
        {
            Id = id;
            SeatsCount = seatscount;
            Destination = destination;
            StartLocation = startlocation;
            Departure = departure;
            Passengers = passengers;
        }
    }
}