using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace TecAlliance.Carpool.Business.Models
{
    public class CarpoolUnitDto
    {
        public long Id { get; set; }
        public int PassengerCount { get; set; }
        public string Destination { get; set; }
        public string StartLocation { get; set; }
        public string Departure { get; set; }
        public List<long> Passengers { get; set; }

        public CarpoolUnitDto(long id, int passengercount, string destination, string startlocation, string departure, List<long> passengers)
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