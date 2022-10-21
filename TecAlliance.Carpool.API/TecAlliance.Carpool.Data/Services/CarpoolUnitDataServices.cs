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
        public bool CheckIfCarpoolUnitExists(int id)
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
        public CarpoolUnit? FilterCarpoolUnitListForSpecificCarpoolUnit(int id)
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
            foreach(int passenger in carpoolUnit.Passengers)
            {
                newLine += $";{passenger}";
            }
            newLine += "\n";
            File.AppendAllText("C:\\010Projects\\019 Fahrgemeinschaft\\Fahrgemeinschaftsapp\\CarpoolList.csv", newLine);            
        }

        public List<CarpoolUnit>? CreateCarpoolUnitListFromFile()
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

        public void UpdateCarpoolUnit(CarpoolUnit carpoolUnit)
        {
            DeleteCarpoolUnitFromFile(carpoolUnit.Id);
            PrintCarpoolUnitToFile(carpoolUnit);
        }

        public void DeleteCarpoolUnitFromFile(int id)
        {
            string[] lines = File.ReadAllLines($"C:\\010Projects\\019 Fahrgemeinschaft\\Fahrgemeinschaftsapp\\CarpoolList.csv");
            List<string> linesToWrite = new List<string>();

            foreach(string s in lines)
            {
                if (!s.Contains($"{id};"))
                {
                    linesToWrite.Add(s);
                }
            }
            File.WriteAllLines($"C:\\010Projects\\019 Fahrgemeinschaft\\Fahrgemeinschaftsapp\\CarpoolList.csv", linesToWrite);
        }

        public CarpoolUnit? BuildCarpoolUnitFromLine(string line)
        {
            if(line != null)
            {
                string[] info = line.Split(";");
                List<int> people = new List<int>();
                for (int i = 5; i < info.Length; i++)
                {
                     people.Add(int.Parse(info[i]));
                }
                CarpoolUnit carpoolUnit = new CarpoolUnit(int.Parse(info[0]), Convert.ToInt32(info[1]), info[2], info[3], info[4], people);
                return carpoolUnit;
            }
            else
            {
                return null;
            }
        }
    }
}
