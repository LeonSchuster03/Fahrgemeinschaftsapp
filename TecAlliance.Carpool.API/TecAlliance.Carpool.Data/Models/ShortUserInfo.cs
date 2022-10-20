using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TecAlliance.Carpool.Data.Models
{
    public class ShortUserInfo
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public bool CanDrive { get; set; }

        public ShortUserInfo(long id, string name, bool candrive)
        {
            Id = id;
            Name = name;
            CanDrive = candrive;
        }

    }
}
