using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TecAlliance.Carpool.Business.Models;
using Swashbuckle.AspNetCore.Filters;



namespace TecAlliance.Carpool.Business.SampleData
{
    public class ShortUserInfoDtoProvider : IExamplesProvider<ShortUserInfoDto>
    {
        public ShortUserInfoDto GetExamples()
        {
            ShortUserInfoDto shortUserInfoDto = new ShortUserInfoDto(0, "Leon", true);
            return shortUserInfoDto;
        }
    }
}
