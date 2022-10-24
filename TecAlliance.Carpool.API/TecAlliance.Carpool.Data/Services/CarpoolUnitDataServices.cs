using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TecAlliance.Carpool.Data.Models;

namespace TecAlliance.Carpool.Data.Services
{
    public class CarpoolUnitDataServices
    {

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

        /// <summary>
        /// Converts the carpool information and prints it into the file
        /// </summary>
        /// <param name="carpoolUnit"></param>
        public void PrintCarpoolUnitToFile(CarpoolUnit carpoolUnit)
        {
            string path = Directory.GetCurrentDirectory();
            var newLine = $"{carpoolUnit.Id};{carpoolUnit.SeatsCount};{carpoolUnit.Destination};{carpoolUnit.StartLocation};{carpoolUnit.Departure}";
            foreach(int passenger in carpoolUnit.Passengers)
            {
                newLine += $";{passenger}";
            }
            newLine += "\n";
            File.AppendAllText(path + "\\..\\TecAlliance.Carpool.Data\\CarpoolList.csv", newLine);            
        }

        /// <summary>
        /// Reads the file and returns a list with all existing carpools
        /// </summary>
        /// <returns></returns>
        public List<CarpoolUnit>? CreateCarpoolUnitListFromFile()
        {
            string path = Directory.GetCurrentDirectory();
            List<CarpoolUnit> carpoolList = new List<CarpoolUnit>();
            string[] fileText = File.ReadAllLines(path + "\\..\\TecAlliance.Carpool.Data\\CarpoolList.csv");

            foreach(string carpoolUnitText in fileText)
            {
                CarpoolUnit carpoolUnit = BuildCarpoolUnitFromLine(carpoolUnitText);
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
            DeleteCarpoolUnitFromFile(carpoolUnit.Id);
            PrintCarpoolUnitToFile(carpoolUnit);
        }


        /// <summary>
        /// Deletes all information about a carpool
        /// </summary>
        /// <param name="id"></param>
        public void DeleteCarpoolUnitFromFile(int id)
        {
            string path = Directory.GetCurrentDirectory();
            string[] lines = File.ReadAllLines(path + "\\..\\TecAlliance.Carpool.Data\\CarpoolList.csv");
            List<string> linesToWrite = new List<string>();

            foreach(string s in lines)
            {
                if (!s.Contains($"{id};"))
                {
                    linesToWrite.Add(s);
                }
            }
            File.WriteAllLines(path + "\\..\\TecAlliance.Carpool.Data\\CarpoolList.csv", linesToWrite);
        }

        /// <summary>
        /// Returns a CarpoolUnit, by processing the given information
        /// </summary>
        /// <param name="line"></param>
        /// <returns></returns>
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
