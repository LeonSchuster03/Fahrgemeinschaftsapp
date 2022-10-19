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
            long id = 0;
            do
            {
                if (!carpoolUnitDataServices.CheckIfCarpoolUnitExists(id))
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

        public void CreateCarpoolUnit(CarpoolUnitDto carpoolUnitDto)
        {
            var carpoolUnit = ConvertCarpoolUnitDtoToCarpoolUnit(carpoolUnitDto);
            carpoolUnitDataServices.CreateCarpoolUnit(carpoolUnit);
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
