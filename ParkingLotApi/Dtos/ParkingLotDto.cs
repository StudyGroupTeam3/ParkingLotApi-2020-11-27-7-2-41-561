using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;

namespace ParkingLotApi.Dtos
{
    public class ParkingLotDto
    {
        public string Name { get; set; }
        public int Capacity { get; set; }
        public string Location { get; set; }
    }
}
