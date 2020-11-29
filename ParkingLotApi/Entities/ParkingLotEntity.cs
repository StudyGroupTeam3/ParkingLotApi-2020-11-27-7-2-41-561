using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;
using ParkingLotApi.Dtos;

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

        [Key]
        public string Name { get; set; }
        public int? Capacity { get; set; }
        public string Location { get; set; }
        [ForeignKey("ParkingLotName")]
        public List<ParkingOrderEntity> Orders { get; set; }
    }
}
