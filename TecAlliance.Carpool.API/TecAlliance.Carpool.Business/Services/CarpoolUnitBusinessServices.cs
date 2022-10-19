using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TecAlliance.Carpool.Business.Models;
using TecAlliance.Carpool.Data.Models;
using TecAlliance.Carpool.Data.Services;

namespace TecAlliance.Carpool.Business.Services
{
    public class CarpoolUnitBusinessServices
    {
        CarpoolUnitDataServices carpoolUnitDataServices;
        public CarpoolUnitBusinessServices()
        {
            carpoolUnitDataServices = new CarpoolUnitDataServices();
        }
        public long GetId()
        {
            //File.Create("C:\\010Projects\\019 Fahrgemeinschaft\\Fahrgemeinschaftsapp\\CarpoolList.csv");
            long id = 0;
            do
            {
                if (!CheckIfCarpoolUnitExists(id))
                {
                    break;
                }
                else
                {
                    id++;
                }
            } while (true);
            return id;
        }

        public List<CarpoolUnitDto> GetAllCarpoolUnits()
        {
            List<CarpoolUnitDto> carpoolUnitDtoList = new List<CarpoolUnitDto>();
            List<CarpoolUnit> carpoolUnits = carpoolUnitDataServices.CreateCarpoolUnitListFromFile();
            foreach(CarpoolUnit carpoolUnit in carpoolUnits)
            {
                CarpoolUnitDto carpoolUnitDto = ConvertCarpoolUnitToCarpoolUnitDto(carpoolUnit);
                carpoolUnitDtoList.Add(carpoolUnitDto);
            }
            return carpoolUnitDtoList;
        }
        

        public bool CheckIfCarpoolUnitExists(long id)
        {
            if (carpoolUnitDataServices.FilterCarpoolUnitListForSpecificCarpoolUnit(id) != null)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public void CreateCarpoolUnit(CarpoolUnitDto carpoolUnitDto)
        {
            var carpoolUnit = ConvertCarpoolUnitDtoToCarpoolUnit(carpoolUnitDto);
            carpoolUnitDataServices.PrintCarpoolUnitToFile(carpoolUnit);
        }
        public CarpoolUnitDto? GetCarpoolUnitById(long id)
        {
            CarpoolUnit carpoolUnit = carpoolUnitDataServices.FilterCarpoolUnitListForSpecificCarpoolUnit(id);
            CarpoolUnitDto carpoolUnitDto = ConvertCarpoolUnitToCarpoolUnitDto(carpoolUnit);
            if(carpoolUnitDto != null)
            {
                return carpoolUnitDto;
            }
            else
            {
                return null;
            }                        
        }

        public CarpoolUnit ConvertCarpoolUnitDtoToCarpoolUnit(CarpoolUnitDto carpoolDto)
        {
            var carpoolUnit = new CarpoolUnit(carpoolDto.Id, carpoolDto.PassengerCount, carpoolDto.Destination, carpoolDto.StartLocation, carpoolDto.Departure, carpoolDto.Passengers);
            return carpoolUnit;
        }

        public CarpoolUnitDto ConvertCarpoolUnitToCarpoolUnitDto(CarpoolUnit carpool)
        {
            var carpoolUnitDto = new CarpoolUnitDto(carpool.Id, carpool.PassengerCount, carpool.Destination, carpool.StartLocation, carpool.Departure, carpool.Passengers);
            return carpoolUnitDto;
        }
    }
}
