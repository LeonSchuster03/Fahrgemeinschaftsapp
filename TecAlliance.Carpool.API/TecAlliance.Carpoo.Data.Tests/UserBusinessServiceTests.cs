using Moq;
using System.Reflection;
using TecAlliance.Carpool.Business.Models;
using TecAlliance.Carpool.Business.Services;
using TecAlliance.Carpool.Data.Models;
using TecAlliance.Carpool.Data.Service;
using TecAlliance.Carpool.Data.Services;
using FluentAssertions;

namespace TecAlliance.Carpool.Business.Tests
{
    [TestClass]
    public class UserBusinessServiceTests
    {
        private readonly UserBusinessServices _service;
        private readonly Mock<IUserDataServices> _userDataServicesMock = new Mock<IUserDataServices>();

        public UserBusinessServiceTests()
        {
            _service = new UserBusinessServices(_userDataServicesMock.Object);
        }

        [TestMethod]
        public void Check_CheckIfUserExists_UserExists()
        {
            //Arrange
            var user1 = new User(0, "Leon", "Leon", "Schuster", 19, "m", "Unterbalbach", "Weikersheim", true);
            _userDataServicesMock.Setup(x => x.FilterUserListForSpecificUser(0)).Returns(user1);

            //Act
            var result = _service.CheckIfUserExists(0);

            //Assert
            result.Equals(true);
        }

        [TestMethod]
        public void Check_CheckIfUserExists_UserDoesNotExist()
        {
            //Arrange
            User? user = null;
            _userDataServicesMock.Setup(x => x.FilterUserListForSpecificUser(0)).Returns(user);

            //Act
            var result = _service.CheckIfUserExists(0);

            //Assert
            result.Equals(true);
        }

        [TestMethod]
        public void Check_ConvertUserToUserDto_ConvertsCorrectly()
        {
            //Arrange
            var user = new User(0, "Leon", "Leon", "Schuster", 19, "m", "Unterbalbach", "Weikersheim", true);
            var userDto = new UserDto(0, "Leon", "Leon", "Schuster", 19, "m", "Unterbalbach", "Weikersheim", true);

            //Act
            var result = _service.ConvertUserToUserDto(user);

            //Assert
            result.Equals(userDto);

        }

        [TestMethod]
        public void Check_ConvertUserDtoToUser_ConvertsCorrectly()
        {
            //Arrange
            var user = new User(0, "Leon", "Leon", "Schuster", 19, "m", "Unterbalbach", "Weikersheim", true);
            var userDto = new UserDto(0, "Leon", "Leon", "Schuster", 19, "m", "Unterbalbach", "Weikersheim", true);

            //Act
            var result = _service.ConvertUserDtoToUser(userDto);

            //Assert
            result.Equals(user);
        }

        [TestMethod]
        public void Check_DeleteUser_TryWithExistingUser()
        {
            //Arrange
            var user = new User(0, "Leon", "Leon", "Schuster", 19, "m", "Unterbalbach", "Weikersheim", true);
            _userDataServicesMock.Setup(x => x.FilterUserListForSpecificUser(0)).Returns(user);

            //Act
            var result = _service.DeleteUser(0);

            //Assert
            result.Equals(true);
        }

        [TestMethod]
        public void Check_DeleteUser_TryWithNotExistingUser()
        {
            //Arrange
            User? user = null;
            _userDataServicesMock.Setup(x => x.FilterUserListForSpecificUser(0)).Returns(user);

            //Act
            var result = _service.DeleteUser(0);

            //Assert
            result.Equals(false);
        }

        [TestMethod]
        public void Check_UpdateUser_TryWithExistingUser()
        {
            //Arrange
            var user = new User(0, "Leon", "Leon", "Schuster", 19, "m", "Unterbalbach", "Weikersheim", true);
            _userDataServicesMock.Setup(x => x.FilterUserListForSpecificUser(0)).Returns(user);
            //Act
            _service.UpdateUser(0, "Leon", "Leon", "Schuster", 19, "m", "Unterbalbach", "Weikersheim", true);

            //Assert
            _userDataServicesMock.Verify(d => d.UpdateUser(It.IsAny<User>()), Times.Once);
            _userDataServicesMock.Verify(d => d.UpdateUser(It.Is<User>(u => u.Id == user.Id)), Times.Once);
        }

        [TestMethod]
        public void Check_UpdateUser_TryWithNonExistingUser()
        {
            //Arrange
            var user = new User(0, "Leon", "Leon", "Schuster", 19, "m", "Unterbalbach", "Weikersheim", true);
            _userDataServicesMock.Setup(x => x.FilterUserListForSpecificUser(0)).Returns(user);
            //Act
            var result = _service.UpdateUser(1, "Leon", "Leon", "Schuster", 19, "m", "Unterbalbach", "Weikersheim", true);

            //Assert
            Assert.IsNull(result);
        }

        [TestMethod]
        public void Check_GetShortUserInfo_ValidId()
        {
            //Arrange
            var userDto = new UserDto(0, "Leon", "Leon", "Schuster", 19, "m", "Unterbalbach", "Weikersheim", true);
            var user = new User(0, "Leon", "Leon", "Schuster", 19, "m", "Unterbalbach", "Weikersheim", true);
            var shortUsuserDto = new ShortUserInfoDto(0, "Leon", true);
            var userList = new List<User>();
            userList.Add(user);

            _userDataServicesMock.Setup(x => x.CreateUserList()).Returns(userList);

            //Act
            ShortUserInfoDto? result = _service.GetShortUserInfo(userDto.Id);

            //Assert
            result.Equals(shortUsuserDto);

        }

        [TestMethod]
        public void Check_GetShortUserInfo_InvalidId()
        {
            //Arrange
            var userDto = new UserDto(0, "Leon", "Leon", "Schuster", 19, "m", "Unterbalbach", "Weikersheim", true);
            var user = new User(0, "Leon", "Leon", "Schuster", 19, "m", "Unterbalbach", "Weikersheim", true);
            var shortUsuserDto = new ShortUserInfoDto(0, "Leon", true);

            var userList = new List<User>();
            userList.Add(user);

            _userDataServicesMock.Setup(x => x.CreateUserList()).Returns(userList);

            //Act
            ShortUserInfoDto? result = _service.GetShortUserInfo(1);

            //Assert
            Assert.IsNull(result);
        }

        [TestMethod]
        public void GetUsersWithSameDestination_DestinationIsMatch()
        {
            //Arrange
            List<User> userList = new List<User>();
            var user1 = new User(0, "Leon", "Leon", "Schuster", 19, "m", "Unterbalbach", "Weikersheim", true);
            var user2 = new User(1, "Simon", "Simon", "Schuster", 19, "m", "Unterbalbach", "Würzburg", true);
            var user3 = new User(2, "Noah", "Noah", "Schuster", 15, "m", "Unterbalbach", "Weikersheim", false);
            userList.Add(user1);
            userList.Add(user2);
            userList.Add(user3);

            List<ShortUserInfoDto> shortUserInfoList = new List<ShortUserInfoDto>();
            var shortUsuserDto1 = new ShortUserInfoDto(0, "Leon", true);
            var shortUsuserDto2 = new ShortUserInfoDto(2, "Noah", false);
            shortUserInfoList.Add(shortUsuserDto1);
            shortUserInfoList.Add(shortUsuserDto2);

            _userDataServicesMock.Setup(x => x.CreateUserList()).Returns(userList);

            //Act
            List<ShortUserInfoDto> result = _service.GetUsersWithSameDestination("Weikersheim");

            //Assert
            result.Should().BeEquivalentTo(shortUserInfoList);
        }

        [TestMethod]
        public void GetUsersWithSameDestination_NoMatch_ReturnsNull()
        {
            //Arrange
            List<User> userList = new List<User>();
            var user1 = new User(0, "Leon", "Leon", "Schuster", 19, "m", "Unterbalbach", "Weikersheim", true);
            var user2 = new User(1, "Simon", "Simon", "Schuster", 19, "m", "Unterbalbach", "Würzburg", true);
            var user3 = new User(2, "Noah", "Noah", "Schuster", 15, "m", "Unterbalbach", "Weikersheim", false);
            userList.Add(user1);
            userList.Add(user2);
            userList.Add(user3);
            List<ShortUserInfoDto> shortUserInfoList = new List<ShortUserInfoDto>();
            _userDataServicesMock.Setup(x => x.CreateUserList()).Returns(userList);

            //Act
            List<ShortUserInfoDto> result = _service.GetUsersWithSameDestination("Bad Mergentheim");

            //Assert
            result.Equals(shortUserInfoList);
        }
    }
}