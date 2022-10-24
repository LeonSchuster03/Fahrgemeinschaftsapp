namespace TecAlliance.Carpool.Data.Models
{
    public class User
    {
        public int Id { get; set; } //sets and returns the Id of an User
        public string? UserName { get; set; } //sets and returns the Username of an User
        public string? FirstName { get; set; } //Sets and returns the first name of an User
        public string? LastName { get; set; } //sets and returns the last name of an User
        public int Age { get; set; } //sets and returns the age of an User
        public string? Gender { get; set; } //sets and returns the gender of an User
        public string? StartPlace { get; set; } //sets and returns the place an User wants to start his carpool
        public string? EndPlace { get; set; } //sets and returns the destination of an User
        public bool HasCar { get; set; } //sets and returns a bool, whether the user has a car/can drive

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <param name="userName"></param>
        /// <param name="firstName"></param>
        /// <param name="lastName"></param>
        /// <param name="age"></param>
        /// <param name="gender"></param>
        /// <param name="startPlace"></param>
        /// <param name="endPlace"></param>
        /// <param name="hasCar"></param>
        public User(int id, string userName, string firstName, string lastName, int age, string gender, string startPlace, string endPlace, bool hasCar)
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