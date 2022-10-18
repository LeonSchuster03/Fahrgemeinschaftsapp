namespace TecAlliance.Carpool.Api.Models
{
    public class User
    {
        public string? Username { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public int Age { get; set; }
        public string? Gender { get; set; }
        public string? StartPlace { get; set; }
        public string? EndPlace { get; set; }
        public bool HasCar { get; set; }

    }
}
