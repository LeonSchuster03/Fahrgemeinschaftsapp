using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TecAlliance.Carpool.Business.Models;
using Swashbuckle.AspNetCore.Filters;



namespace TecAlliance.Carpool.Business.SampleData
{
    public class ListUserDtoProvider : IExamplesProvider<List<UserDto>>
    {
        public List<UserDto> GetExamples()
        {
            List<UserDto> examples = new List<UserDto>();
            UserDto userdto1 = new UserDto(0, "Leon", "Leon", "Schuster", 19, "m", "Unterbalbach", "Weikersheim", true);
            UserDto userdto2 = new UserDto(1, "Philipp", "Philipp", "Ehinger", 26, "m", "Bad Mergentheim", "Weikersheim", true);
            UserDto userdto3 = new UserDto(1, "Marcello", "Marcello", "Greulich", 18, "m", "Schweinberg", "Weikersheim", false);
            examples.Add(userdto1);
            examples.Add(userdto2);
            examples.Add(userdto3);
            return examples;
        }
    }
}
