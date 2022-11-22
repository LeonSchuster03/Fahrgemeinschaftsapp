﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TecAlliance.Carpool.Data.Models;

namespace TecAlliance.Carpool.Data.Services
{
    public interface ICarpoolUnitDataServices
    {


        /// <summary>
        /// Takes the id and searches the carpoollist for a carpool with this id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public CarpoolUnit? FilterCarpoolUnitListForSpecificCarpoolUnit(int id);


        /// <summary>
        /// Converts the carpool information and stores it in the storage unit
        /// </summary>
        /// <param name="carpoolUnit"></param>
        void PrintCarpoolUnit(CarpoolUnit carpoolUnit);


        /// <summary>
        /// Reads all stored information and returns a list with all existing carpools
        /// </summary>
        /// <returns></returns>
        List<CarpoolUnit>? CreateCarpoolUnitList();


        /// <summary>
        /// Replaces old information with new information
        /// </summary>
        /// <param name="carpoolUnit"></param>
        void UpdateCarpoolUnit(CarpoolUnit carpoolUnit);

        /// <summary>
        /// tbd
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="carpoolId"></param>
        void AddPassengerToCarpool(int carpoolId, int userId);

        /// <summary>
        /// tbd
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="carpoolId"></param>
        void RemovePassengerFromCarpool(int carpoolId, int userId);


        /// <summary>
        /// Deletes all information about a carpool
        /// </summary>
        /// <param name="id"></param>
        void DeleteCarpoolUnit(int id);


        /// <summary>
        /// tbd
        /// </summary>
        /// <returns></returns>
        int GetNewId();

        /// <summary>
        /// tbd
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        CarpoolUnit GetCarpoolById(int id);

    }
}