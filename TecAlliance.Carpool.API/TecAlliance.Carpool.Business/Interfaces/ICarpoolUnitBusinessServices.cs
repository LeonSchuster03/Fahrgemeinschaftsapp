using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using TecAlliance.Carpool.Business.Models;
using TecAlliance.Carpool.Data.Models;
using TecAlliance.Carpool.Data.Services;

namespace TecAlliance.Carpool.Business.Services
{
    public interface ICarpoolUnitBusinessServices
    {


        /// <summary>
        /// Generates unique Id
        /// </summary>
        /// <returns></returns>
        public int GetId();

        /// <summary>
        /// Creates list of all existing carpools
        /// </summary>
        /// <returns></returns>
        public List<CarpoolUnitDto> GetAllCarpoolUnits();

        /// <summary>
        /// Creates list of all users in a specific carpool
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public List<int> GetUsersInCarpool(int id);

        /// <summary>
        /// Selects carpool with specific id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public CarpoolUnitDto? SelectSpecificCarpool(int id);

        /// <summary>
        /// Updates information of a carpool
        /// </summary>
        /// <param name="carpoolUnitDto"></param>
        /// <exception cref="Exception"></exception>
        public void UpdateCarpoolUnit(CarpoolUnitDto carpoolUnitDto);

        /// <summary>
        /// Removes all information of a carpool
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public bool DeleteCarpoolUnit(int id);

        /// <summary>
        /// Checks, if a carpool with specific id exists
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public bool CheckIfCarpoolUnitExists(int id);

        /// <summary>
        /// Creates a new carpool
        /// </summary>
        /// <param name="carpoolUnitDto"></param>
        public CarpoolUnitDto CreateCarpoolUnit(int id, int seatsCount, string destination, string startLocation, string departure, List<int> passengers);

        /// <summary>
        /// Returns a carpool with the given Id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public CarpoolUnitDto? GetCarpoolUnitById(int id);

        /// <summary>
        /// User enters carpool by providing his user Id and the carpool Id
        /// </summary>
        /// <param name="cpId"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        public CarpoolUnitDto? JoinCarpoolUnit(int cpId, int userId);
        

        /// <summary>
        /// User leaves carpool by providing his user Id and the carpool Id
        /// </summary>
        /// <param name="cpId"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        public CarpoolUnitDto? LeaveCarpoolUnit(int cpId, int userId);
                
    }
}
