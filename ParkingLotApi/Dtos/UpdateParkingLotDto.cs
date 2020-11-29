using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using ParkingLotApi.Entities;

namespace ParkingLotApi.Dtos
{
    public class UpdateParkingLotDto
    {
        public UpdateParkingLotDto()
        {
        }

        public UpdateParkingLotDto(ParkingLotEntity parkingLotEntity)
        {
            this.Name = parkingLotEntity.Name;
            this.Capacity = parkingLotEntity.Capacity;
            this.Location = parkingLotEntity.Location;
        }

        public string Name { get; set; }
        public int? Capacity { get; set; }
        public string Location { get; set; }
    }
}
