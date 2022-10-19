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
            if (FilterCarpoolUnitListForSpecificCarpoolUnit(id) != null)
            {
                return true;
            }
            else
            {
                return false;
            }           
        }
        public CarpoolUnit? FilterCarpoolUnitListForSpecificCarpoolUnit(long id)
        {
            List<CarpoolUnit> carpoolUnitList = CreateCarpoolUnitListFromFile();
            foreach(CarpoolUnit carpoolUnit in carpoolUnitList)
            {
                if(carpoolUnit.Id == id)
                {
                    return carpoolUnit;
                }
            }
            return null;
        }

        public void PrintCarpoolUnitToFile(CarpoolUnit carpoolUnit)
        {           
            var newLine = $"{carpoolUnit.Id};{carpoolUnit.PassengerCount};{carpoolUnit.Destination};{carpoolUnit.StartLocation};{carpoolUnit.Departure}";
            foreach(long passenger in carpoolUnit.Passengers)
            {
                newLine += $";{passenger}";
            }
            newLine += "\n";
            File.AppendAllText("C:\\010Projects\\019 Fahrgemeinschaft\\Fahrgemeinschaftsapp\\CarpoolList.csv", newLine);            
        }

        public List<CarpoolUnit> CreateCarpoolUnitListFromFile()
        {
            List<CarpoolUnit> carpoolList = new List<CarpoolUnit>();
            string[] fileText = File.ReadAllLines("C:\\010Projects\\019 Fahrgemeinschaft\\Fahrgemeinschaftsapp\\CarpoolList.csv");

            foreach(string carpoolUnitText in fileText)
            {
                CarpoolUnit carpoolUnit = BuildCarpoolUnitFromLine(carpoolUnitText);
                carpoolList.Add(carpoolUnit);
            }
            return carpoolList;
        }

        public CarpoolUnit? BuildCarpoolUnitFromLine(string line)
        {
            if(line != null)
            {
                string[] info = line.Split(";");
                List<long> people = new List<long>();
                for (int i = 5; i < info.Length-5; i++)
                {
                     people.Append(long.Parse(info[i]));
                }
                CarpoolUnit carpoolUnit = new CarpoolUnit(long.Parse(info[0]), Convert.ToInt32(info[1]), info[2], info[3], info[4], people);
                return carpoolUnit;
            }
            else
            {
                return null;
            }
        }
    }
}
