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

        /// <summary>
        /// Generates unique Id
        /// </summary>
        /// <returns></returns>
        public int GetId()
        {
            int id = 0;
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

        /// <summary>
        /// Creates list of all existing carpools
        /// </summary>
        /// <returns></returns>
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

        /// <summary>
        /// Creates list of all users in a specific carpool
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public List<int> GetUsersInCarpool(int id)
        {
            CarpoolUnitDto carpoolUnitDto = SelectSpecificCarpool(id);
            if(carpoolUnitDto == null)
            {
                return null;
            }
            List<int> usersInCarpoolList = carpoolUnitDto.Passengers;
            return usersInCarpoolList;
        }

        /// <summary>
        /// Selects carpool with specific id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public CarpoolUnitDto? SelectSpecificCarpool(int id)
        {
            CarpoolUnitDto selectedCarpoolUnitDto; 
            List<CarpoolUnitDto> carpoolUnitDtoList = GetAllCarpoolUnits();
            foreach(CarpoolUnitDto carpoolUnitDto in carpoolUnitDtoList)
            {
                if(carpoolUnitDto.Id == id)
                {
                    selectedCarpoolUnitDto = carpoolUnitDto;
                    return selectedCarpoolUnitDto;
                }
            }
            return null;
        }
        
        /// <summary>
        /// Updates information of a carpool
        /// </summary>
        /// <param name="carpoolUnitDto"></param>
        /// <exception cref="Exception"></exception>
        public void UpdateCarpoolUnit(CarpoolUnitDto carpoolUnitDto)
        {
            CarpoolUnit carpoolUnit = ConvertCarpoolUnitDtoToCarpoolUnit(carpoolUnitDto);
            if (CheckIfCarpoolUnitExists(carpoolUnit.Id))
            {
                carpoolUnitDataServices.UpdateCarpoolUnit(carpoolUnit);
            }
            else
            {
                throw new Exception();
            }
        }

        /// <summary>
        /// Removes all information of a carpool
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public bool DeleteCarpoolUnit(int id)
        {
            if (CheckIfCarpoolUnitExists(id))
            {
                carpoolUnitDataServices.DeleteCarpoolUnitFromFile(id);
                return true;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// Checks, if a carpool with specific id exists
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public bool CheckIfCarpoolUnitExists(int id)
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

        /// <summary>
        /// Creates a new carpool
        /// </summary>
        /// <param name="carpoolUnitDto"></param>
        public void CreateCarpoolUnit(CarpoolUnitDto carpoolUnitDto)
        {
            var carpoolUnit = ConvertCarpoolUnitDtoToCarpoolUnit(carpoolUnitDto);
            carpoolUnitDataServices.PrintCarpoolUnitToFile(carpoolUnit);
        }


        public CarpoolUnitDto? GetCarpoolUnitById(int id)
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

        /// <summary>
        /// User enters carpool by providing his user Id and the carpool Id
        /// </summary>
        /// <param name="cpId"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        public CarpoolUnitDto? JoinCarpoolUnit(int cpId, int userId)
        {          
            if(CheckIfCarpoolUnitExists(cpId))
            {
                CarpoolUnitDto carpoolUnitDto = GetCarpoolUnitById(cpId);
                if (carpoolUnitDto.PassengerCount > carpoolUnitDto.Passengers.Count() && !carpoolUnitDto.Passengers.Contains(userId))
                {
                        carpoolUnitDto.Passengers.Add(userId);
                        UpdateCarpoolUnit(carpoolUnitDto);
                        return carpoolUnitDto;
                }
                else
                {
                    return null;
                }
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// User leaves carpool by providing his user Id and the carpool Id
        /// </summary>
        /// <param name="cpId"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        public CarpoolUnitDto? LeaveCarpoolUnit(int cpId, int userId)
        {
            if (CheckIfCarpoolUnitExists(cpId))
            {
                CarpoolUnitDto carpoolUnitDto = GetCarpoolUnitById(cpId);
                carpoolUnitDto.Passengers.Remove(userId);
                if (carpoolUnitDto.Passengers.Count() != 0)
                {
                    UpdateCarpoolUnit(carpoolUnitDto);
                    return carpoolUnitDto;
                }
                else
                {
                    DeleteCarpoolUnit(cpId);
                    return null;
                }
                
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// Converts a CarpoolUnitDto to a CarpoolUnit
        /// </summary>
        /// <param name="carpoolDto"></param>
        /// <returns></returns>
        public CarpoolUnit ConvertCarpoolUnitDtoToCarpoolUnit(CarpoolUnitDto carpoolDto)
        {
            var carpoolUnit = new CarpoolUnit(carpoolDto.Id, carpoolDto.PassengerCount, carpoolDto.Destination, carpoolDto.StartLocation, carpoolDto.Departure, carpoolDto.Passengers);
            return carpoolUnit;
        }

        /// <summary>
        /// Converts a CarpoolUnit to a CarpoolUnitDto
        /// </summary>
        /// <param name="carpool"></param>
        /// <returns></returns>
        public CarpoolUnitDto ConvertCarpoolUnitToCarpoolUnitDto(CarpoolUnit carpool)
        {
            var carpoolUnitDto = new CarpoolUnitDto(carpool.Id, carpool.PassengerCount, carpool.Destination, carpool.StartLocation, carpool.Departure, carpool.Passengers);
            return carpoolUnitDto;
        }
    }
}
