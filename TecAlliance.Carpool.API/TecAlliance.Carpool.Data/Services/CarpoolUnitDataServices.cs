using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TecAlliance.Carpool.Data.Models;

namespace TecAlliance.Carpool.Data.Services
{
    public class CarpoolUnitDataServices : ICarpoolUnitDataServices
    {
        public string Path { get; set; }

        public CarpoolUnitDataServices()
        {
            Path = Directory.GetCurrentDirectory() + "\\..\\TecAlliance.Carpool.Data\\Carpoollist.csv";
        }
        /// <summary>
        /// checks if a carpool with the given Id exists
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
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

        /// <summary>
        /// Takes the id and searches the carpoollist for a carpool with this id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public CarpoolUnit? FilterCarpoolUnitListForSpecificCarpoolUnit(int id)
        {
            List<CarpoolUnit> carpoolUnitList = CreateCarpoolUnitList();
            foreach(CarpoolUnit carpoolUnit in carpoolUnitList)
            {
                if(carpoolUnit.Id == id)
                {
                    return carpoolUnit;
                }
            }
            return null;
        }

        /// <summary>
        /// Converts the carpool information and prints it into the file
        /// </summary>
        /// <param name="carpoolUnit"></param>
        public void PrintCarpoolUnit(CarpoolUnit carpoolUnit)
        {
            var newLine = $"{carpoolUnit.Id};{carpoolUnit.SeatsCount};{carpoolUnit.Destination};{carpoolUnit.StartLocation};{carpoolUnit.Departure}";
            foreach(int passengerID in carpoolUnit.Passengers)
            {
                newLine += $";{passengerID}";
            }
            newLine += "\n";
            File.AppendAllText(Path, newLine);            
        }

        /// <summary>
        /// Reads the file and returns a list with all existing carpools
        /// </summary>
        /// <returns></returns>
        public List<CarpoolUnit>? CreateCarpoolUnitList()
        {
            List<CarpoolUnit> carpoolList = new List<CarpoolUnit>();
            string[] fileText = File.ReadAllLines(Path);

            foreach(string carpoolUnitText in fileText)
            {
                CarpoolUnit carpoolUnit = BuildCarpoolUnit(carpoolUnitText);
                carpoolList.Add(carpoolUnit);
            }
            return carpoolList;
        }
        

        /// <summary>
        /// Replaces old information with new information
        /// </summary>
        /// <param name="carpoolUnit"></param>
        public void UpdateCarpoolUnit(CarpoolUnit carpoolUnit)
        {
            DeleteCarpoolUnit(carpoolUnit.Id);
            PrintCarpoolUnit(carpoolUnit);
        }


        /// <summary>
        /// Deletes all information about a carpool
        /// </summary>
        /// <param name="id"></param>
        public void DeleteCarpoolUnit(int id)
        {
            
            var allCarpools = CreateCarpoolUnitList();
            using (File.Create(Path));
            foreach (var carpool in allCarpools)
            {
                if(carpool.Id != id)
                {
                    PrintCarpoolUnit(carpool);
                }
            }
        }

        /// <summary>
        /// Returns a CarpoolUnit, by processing the given information
        /// </summary>
        /// <param name="line"></param>
        /// <returns></returns>
        public CarpoolUnit? BuildCarpoolUnit(string line)
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
