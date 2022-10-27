using FluentAssertions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TecAlliance.Carpool.Data.Models;
using TecAlliance.Carpool.Data.Services;

namespace TecAlliance.Carpool.Data.Tests
{
    [TestClass]
    public class CarpoolUnitDataServiceTests
    {
        private CarpoolUnitDataServices PrepareCarpoolUnitDataServicesTestObject(List<CarpoolUnit> carpoolUnitList)
        {
            var carpoolUnitDataServices = new CarpoolUnitDataServices();
            carpoolUnitDataServices.Path = Directory.GetCurrentDirectory() + "\\..\\..\\.\\Test-CarpoolUnitlist.csv";
            using (File.Create(carpoolUnitDataServices.Path)) { }
            foreach (var carpoolUnit in carpoolUnitList)
            {
                carpoolUnitDataServices.PrintCarpoolUnitToFile(carpoolUnit);
            }
            return carpoolUnitDataServices;
        }

        private List<CarpoolUnit> PrepareCarpoolUnitList()
        {
            var passengers = new List<int>() { 1, 2 };
            var carpoolUnit1 = new CarpoolUnit(0, 2, "Weikersheim", "Unterbalbach", "6:45", passengers);
            var carpoolUnit2 = new CarpoolUnit(1, 2, "Würzburg", "Bad Mergentheim", "17.10", passengers);
            var carpoolUnitList = new List<CarpoolUnit>() { carpoolUnit1, carpoolUnit2};
            return carpoolUnitList;
        }

        [TestMethod]
        public void CheckCreateCarpoolUnitList()
        {
            //Arrange
            var carpoolUnitList = PrepareCarpoolUnitList();
            var carpoolDataServices = PrepareCarpoolUnitDataServicesTestObject(carpoolUnitList);
            var expected = carpoolUnitList;

            //Act
            var actual = carpoolDataServices.CreateCarpoolUnitListFromFile();

            //Assert
            actual.Should().BeEquivalentTo(expected);
        }

        [TestMethod]
        public void CheckAddCarpoolUnit()
        {
            //Arrange
            var carpoolUnitList = PrepareCarpoolUnitList();
            var carpoolDataServices = PrepareCarpoolUnitDataServicesTestObject(carpoolUnitList);
            var passengers = new List<int>() { 1, 2 };
            var carpoolUnit2 = new CarpoolUnit(2, 2, "Bad Mergentheim", "Tauberbischofsheim", "12:45", passengers);
            carpoolUnitList.Add(carpoolUnit2);
            var expected = carpoolUnitList;

            //Act
            carpoolDataServices.PrintCarpoolUnitToFile(carpoolUnit2);
            var actual = carpoolDataServices.CreateCarpoolUnitListFromFile();

            //Assert
            actual.Should().BeEquivalentTo(expected);
        }

        [TestMethod]
        public void CheckDeleteCarpoolUnit()
        {
            //Arrange
            
            var carpoolUnitList = PrepareCarpoolUnitList();
            var carpoolDataServices = PrepareCarpoolUnitDataServicesTestObject(carpoolUnitList);
            var passengers = new List<int>() { 1, 2 };
            var carpoolUnit1 = carpoolUnitList[1];
            carpoolUnitList.RemoveAt(1);
            var expected = carpoolUnitList;

            //Act
            carpoolDataServices.DeleteCarpoolUnitFromFile(carpoolUnit1.Id);
            var actual = carpoolDataServices.CreateCarpoolUnitListFromFile();

            //Assert
            actual.Should().BeEquivalentTo(expected);
        }

        [TestMethod]
        public void CheckSelectSpecificCarpoolUnit()
        {
            //Arrange
            var carpoolUnitList = PrepareCarpoolUnitList();
            var carpoolDataServices = PrepareCarpoolUnitDataServicesTestObject(carpoolUnitList);
            var passengers = new List<int>() { 1, 2 };
            var expected = new CarpoolUnit(0, 2, "Weikersheim", "Unterbalbach", "6:45", passengers);

            //Act
            var actual = carpoolDataServices.FilterCarpoolUnitListForSpecificCarpoolUnit(expected.Id);

            //Arrange
            actual.Should().BeEquivalentTo(expected);
        }

        [TestMethod]
        public void CheckUpdateUser()
        {
            //Arrange
            var carpoolUnitList = PrepareCarpoolUnitList();
            var carpoolUnitDataService = PrepareCarpoolUnitDataServicesTestObject(carpoolUnitList);
            carpoolUnitList.RemoveAt(1);
            var passengers = new List<int>() { 1, 2 };
            var carpoolUnit1 = new CarpoolUnit(1, 2, "Lauda", "Edelfingen", "6:45", passengers);
            carpoolUnitList.Add(carpoolUnit1);
            var expected = carpoolUnitList;

            //Act
            carpoolUnitDataService.UpdateCarpoolUnit(carpoolUnit1);
            var actual = carpoolUnitDataService.CreateCarpoolUnitListFromFile();

            //Assert
            actual.Should().BeEquivalentTo(expected);
        }

        [TestMethod]
        public void CheckBuildCarpoolUnit()
        {
            //Arrange
            var carpoolUnitList = PrepareCarpoolUnitList();
            var carpoolUnitDataService = PrepareCarpoolUnitDataServicesTestObject(carpoolUnitList);
            var passengers = new List<int>() { 1, 2 };
            var expected = new CarpoolUnit(0, 2, "Weikersheim", "Unterbalbach", "7:30", passengers);

            //Act
            var actual = carpoolUnitDataService.BuildCarpoolUnitFromLine("0;2;Weikersheim;Unterbalbach;7:30;1;2");
            
            //Assert
            actual.Should().BeEquivalentTo(expected);
        }
    }
}
