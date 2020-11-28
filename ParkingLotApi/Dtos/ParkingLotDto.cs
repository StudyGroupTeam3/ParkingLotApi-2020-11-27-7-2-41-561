using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ParkingLotApi.Entities;

namespace ParkingLotApi.Dtos
{
    public class ParkingLotDto
    {
        public ParkingLotDto()
        {
        }

        public ParkingLotDto(ParkingLotEntity parkingLotEntity)
        {
            if (parkingLotEntity != null)
            {
                this.Name = parkingLotEntity.Name;
                this.Capacity = parkingLotEntity.Capacity;
                this.Location = parkingLotEntity?.Location;
            }

            else
            {
                this.Name = "default";
                this.Capacity = 0;
                this.Location = "default";
            }
        }

        public string Name { get; set; }
        public int Capacity { get; set; }
        public string Location { get; set; }
    }
}
