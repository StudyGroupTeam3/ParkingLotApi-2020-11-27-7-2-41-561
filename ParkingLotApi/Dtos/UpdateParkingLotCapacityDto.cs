using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ParkingLotApi.Dtos
{
    public class UpdateParkingLotCapacityDto
    {
        public UpdateParkingLotCapacityDto()
        {
        }

        public UpdateParkingLotCapacityDto(int capacity)
        {
            this.Capacity = capacity;
        }

        public int Capacity { get; set; }
    }
}
