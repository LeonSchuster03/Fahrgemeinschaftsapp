using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TecAlliance.Carpool.Data.Models;

namespace TecAlliance.Carpool.Data.Services
{
    public class CarpoolUnitDataServices
    {
        public bool CheckIfCarpoolUnitExists(long id)
        {
            FileInfo fi = new FileInfo($"C:\\010Projects\\019 Fahrgemeinschaft\\Fahrgemeinschaftsapp\\CarpoolUnitList\\{id}.csv");
            if (fi.Exists)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public void PrintCarpoolUnitToFile(CarpoolUnit carpoolUnit)
        {
            using (StreamWriter writer = new StreamWriter($"C:\\010Projects\\019 Fahrgemeinschaft\\Fahrgemeinschaftsapp\\CarpoolUnitList\\{carpoolUnit.Id}.csv"))
            {
                var newLine = $"{carpoolUnit.Id};{carpoolUnit.PassengerCount};{carpoolUnit.Destination};{carpoolUnit.StartLocation};{carpoolUnit.Departure}";
                foreach(long passenger in carpoolUnit.Passengers)
                {
                    newLine += $";{passenger}";
                }
                writer.WriteLine(newLine);
            }
        }

        public void CreateCarpoolUnit(CarpoolUnit carpoolUnit)
        {
            using (FileStream fs = File.Create($"C:\\010Projects\\019 Fahrgemeinschaft\\Fahrgemeinschaftsapp\\CarpoolUnitList\\{carpoolUnit.Id}.csv"));
            PrintCarpoolUnitToFile(carpoolUnit);
        }
    }
}
