using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Threading.Tasks;
using ParkingLotApi.Dtos;

namespace ParkingLotApi.Entities
{
    public class UpdateParkingLotEntity
    {
        public UpdateParkingLotEntity()
        {
        }

        public UpdateParkingLotEntity(UpdateParkingLotDto updateParkingLotDto)
        {
            this.Name = updateParkingLotDto.Name;
            this.Capacity = updateParkingLotDto.Capacity;
            this.Location = updateParkingLotDto.Location;
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public int? Capacity { get; set; }
        public string Location { get; set; }
    }
}
