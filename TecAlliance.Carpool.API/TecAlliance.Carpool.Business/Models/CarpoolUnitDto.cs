using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace TecAlliance.Carpool.Business.Models
{
    public class CarpoolUnitDto
    {
        public int Id { get; set; } //sets and returns the Id of a CarpoolUnitDto
        public int SeatsCount { get; set; } //sets and returns the Number of seats in the carpool
        public string Destination { get; set; } //sets and returns the destination of the carpool
        public string StartLocation { get; set; } //sets and returns the startlocation of the carpool
        public string Departure { get; set; } //sets and returns the time, the carpool starts
        public List<int> Passengers { get; set; } //sets and returns a list of UserIds of the passengers

        public CarpoolUnitDto(int id, int seatscount, string destination, string startlocation, string departure, List<int> passengers)
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