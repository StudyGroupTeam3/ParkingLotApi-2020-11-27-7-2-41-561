using ParkingLotApi.Dtos;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ParkingLotApi.Entities
{
    public class ParkingLotEntity
    {
        public ParkingLotEntity()
        {
        }

        public ParkingLotEntity(ParkingLot parkingLot)
        {
            Name = parkingLot.Name;
            Capacity = parkingLot.Capacity;
            Location = parkingLot.Location;
        }

        [Key]
        public string Name { get; set; }
        public int Capacity { get; set; }
        public string Location { get; set; }
    }
}
