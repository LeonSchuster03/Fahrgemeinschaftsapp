using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace TecAlliance.Carpool.Business.Models
{
    public class CarpoolUnitDto
    {
        public int Id { get; set; }
        public int PassengerCount { get; set; }
        public string Destination { get; set; }
        public string StartLocation { get; set; }
        public string Departure { get; set; }
        public List<int> Passengers { get; set; }

        public CarpoolUnitDto(int id, int passengercount, string destination, string startlocation, string departure, List<int> passengers)
        {
            Id = id;
            PassengerCount = passengercount;
            Destination = destination;
            StartLocation = startlocation;
            Departure = departure;
            Passengers = passengers;
        }
    }
}