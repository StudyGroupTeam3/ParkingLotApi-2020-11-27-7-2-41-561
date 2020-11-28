using ParkingLotApi.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ParkingLotApi.Entities
{
    public class ParkingLotEntity
    {
        public ParkingLotEntity()
        {
        }

        public ParkingLotEntity(ParkingLotDto parkingLotDto)
        {
            this.Name = parkingLotDto.Name;
            this.Capacity = parkingLotDto.Capacity;
            this.Location = parkingLotDto.Location;
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public int Capacity { get; set; }
        public string Location { get; set; }
    }
}
