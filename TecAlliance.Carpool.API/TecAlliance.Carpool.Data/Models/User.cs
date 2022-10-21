namespace TecAlliance.Carpool.Data.Models
{
    public class User
    {
        public int Id { get; set; }
        public string? UserName { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public int Age { get; set; }
        public string? Gender { get; set; }
        public string? StartPlace { get; set; }
        public string? EndPlace { get; set; }
        public bool HasCar { get; set; }

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