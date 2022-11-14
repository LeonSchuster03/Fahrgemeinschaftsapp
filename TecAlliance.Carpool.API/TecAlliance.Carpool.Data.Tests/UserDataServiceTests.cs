using TecAlliance.Carpool.Data;
using TecAlliance.Carpool.Data.Models;
using TecAlliance.Carpool.Data.Service;
using FluentAssertions;
using static System.Net.Mime.MediaTypeNames;

namespace TecAlliance.Carpool.Data.Tests
{
    [TestClass]
    public class UserDataServiceTests 
    {
        private UserDataServices PrepareUserDataServicesTestObject(List<User> userList)
        {
            var userDataService = new UserDataServices();
            userDataService.Path = Directory.GetCurrentDirectory() + "\\..\\..\\.\\Test-Userlist.csv";
            using (File.Create(userDataService.Path)) { }
            foreach(var user in userList) 
            {
                userDataService.PrintUserInfo(user);
            }
            return userDataService;
        }

        private List<User> PrepareUserList()
        {
            var user1 = new User(0, "Leon", "Leon", "Schuster", 19, "m", "Unterbalbach", "Weikersheim", true);
            var user2 = new User(1, "Phillip", "Phillip", "Ehinger", 26, "m", "Würzburg", "Weikersheim", true);
            var userList = new List<User>() { user1, user2};
            return userList;
        }

        [TestMethod]
        public void CheckCreatesUserListCorrectly()
        {
            //Arrange
            var expected = PrepareUserList();
            var userDataService = PrepareUserDataServicesTestObject(expected);

            //Act
            var actual = userDataService.CreateUserList();
            
            //Assert
            actual.Should().BeEquivalentTo(expected);
        }

        [TestMethod]
        public void CheckAddUser()
        {
            //Arrange
            var userList = PrepareUserList();
            var userDataService = PrepareUserDataServicesTestObject(userList);
            var user = new User(2, "Marcello", "Marcello", "Greulich", 18, "m", "Schweinberg", "Weikersheim", true);
            userList.Add(user);
            var expected = userList;

            //Act
            userDataService.PrintUserInfo(user);
            var actual = userDataService.CreateUserList();

            //Assert
            actual.Should().BeEquivalentTo(expected);
        }

        [TestMethod]
        public void CheckDeleteUser()
        {
            //Arrange
            var userList = PrepareUserList();
            var userDataService = PrepareUserDataServicesTestObject(userList);
            var user2 = new User(1, "Phillip", "Phillip", "Ehinger", 26, "m", "Würzburg", "Weikersheim", true);
            userList.RemoveAt(1);
            var expected = userList;

            //Act
            userDataService.DeleteUser(user2.Id);
            var actual = userDataService.CreateUserList();

            //Assert
            actual.Should().BeEquivalentTo(expected);
        }
        [TestMethod]
        public void CheckFilterForSpecificUser()
        {
            //Arrange
            var userList = PrepareUserList();
            var userDataService = PrepareUserDataServicesTestObject(userList);
            var expected = new User(1, "Phillip", "Phillip", "Ehinger", 26, "m", "Würzburg", "Weikersheim", true);

            //Act
            var actual = userDataService.FilterUserListForSpecificUser(1);

            //Assert
            actual.Should().BeEquivalentTo(expected);
        }

        [TestMethod]
        public void CheckUpdateUser()
        {
            //Arrange
            var userList = PrepareUserList();
            var userDataService = PrepareUserDataServicesTestObject(userList);
            userList.RemoveAt(1);
            var user2 = new User(1, "Lukas", "Lukas", "Welker", 18, "m", "Tauberbischofsheim", "Weikersheim", true);
            userList.Add(user2);
            var expected = userList;

            //Act
            userDataService.UpdateUser(user2);
            var actual = userDataService.CreateUserList();

            //Assert
            actual.Should().BeEquivalentTo(expected);
        }

        [TestMethod]
        public void CheckBuildUser()
        {
            //Arrange
            var userList = PrepareUserList();
            var userDataService = PrepareUserDataServicesTestObject(userList);
            var expected = new User(1, "Phillip", "Phillip", "Ehinger", 26, "m", "Würzburg", "Weikersheim", true);

            //Act
            var actual = userDataService.BuildUser("1;Phillip;Phillip;Ehinger;26;m;Würzburg;Weikersheim;true");

            //Assert
            actual.Should().BeEquivalentTo(expected);
        }
    }
}