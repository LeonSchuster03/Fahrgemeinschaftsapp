using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TecAlliance.Carpool.Business.Models
{
    public class UserDto
    {
        
        public int Id { get; set; } //sets and returns the Id of an user
        public string UserName { get; set; } //sets and returns the Username of an user
        public string FirstName { get; set; } //sets and returns the first name of an user
        public string LastName { get; set; } //sets and returns the last name of an user
        public int Age { get; set; } //sets and returns the age of an user
        public string Gender { get; set; } //sets and returns the gender of an user
        public string StartPlace { get; set; } //sets and returns the place an User wants to start his carpool
        public string EndPlace { get; set; } //sets and returns the destination of an User
        public bool HasCar { get; set; } //sets and returns a bool, whether the user has a car/can drive

        public UserDto(int id, string userName, string firstName, string lastName, int age, string gender, string startPlace, string endPlace, bool hasCar)
        {
            Id = id;
            UserName = userName;
            FirstName = firstName;
            LastName = lastName;
            Age = age;
            Gender = gender;
            StartPlace = startPlace;
            EndPlace = endPlace;
            HasCar = hasCar;
        }
    }
}
