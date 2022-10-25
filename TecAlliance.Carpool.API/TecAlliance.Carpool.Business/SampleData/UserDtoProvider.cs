using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TecAlliance.Carpool.Business.Models;
using Swashbuckle.AspNetCore.Filters;



namespace TecAlliance.Carpool.Business.SampleData
{
    public class UserDtoProvider : IExamplesProvider <UserDto>
    {
        public UserDto GetExamples()
        {
            UserDto userdto = new UserDto(0, "Leon", "Leon", "Schuster", 19, "m", "Unterbalbach", "Weikersheim", true);
            return userdto;
        }
    }
}
