using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TecAlliance.Carpool.Business.Models;
using Swashbuckle.AspNetCore.Filters;



namespace TecAlliance.Carpool.Business.SampleData
{
    public class ListCarpoolUnitDtoProvider : IExamplesProvider<List<CarpoolUnitDto>>
    {
        public List<CarpoolUnitDto> GetExamples()
        {
            List<CarpoolUnitDto> examples = new List<CarpoolUnitDto>();
            List<int> ids1 = new List<int>();
            ids1.Add(1);
            ids1.Add(2);
            ids1.Add(3);

            List<int> ids2 = new List<int>();
            ids2.Add(4);
            ids2.Add(5);
            ids2.Add(6);

            CarpoolUnitDto carpoolUnitDto1 = new CarpoolUnitDto(0, 3, "Weikersheim", "Unterbalbach", "7:30", ids1);
            CarpoolUnitDto carpoolUnitDto2 = new CarpoolUnitDto(1, 3, "Würzburg", "Lauda", "16:30", ids2);

            examples.Add(carpoolUnitDto1);
            examples.Add(carpoolUnitDto2);

            return examples;
        }
    }
}