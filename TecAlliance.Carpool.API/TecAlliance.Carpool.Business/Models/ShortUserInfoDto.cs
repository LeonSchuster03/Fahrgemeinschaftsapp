using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TecAlliance.Carpool.Business.Models
{
    public class ShortUserInfoDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public bool CanDrive { get; set; }

        public ShortUserInfoDto(int id, string name, bool candrive)
        {
            Id = id;
            Name = name;
            CanDrive = candrive;
        }
    }
}
