using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TecAlliance.Carpool.Business.Models;
using Swashbuckle.AspNetCore.Filters;



namespace TecAlliance.Carpool.Business.SampleData
{
    public class CarpoolUnitDtoProvider : IExamplesProvider<CarpoolUnitDto>
    {
        public CarpoolUnitDto GetExamples()
        {
            List<int> ids = new List<int>();
            ids.Add(1);
            ids.Add(2);
            ids.Add(3);
            CarpoolUnitDto carpoolUnitDto = new CarpoolUnitDto(0, 3, "Weikersheim", "Unterbalbach", "7:30", ids);
            return carpoolUnitDto;
        }
    }
}