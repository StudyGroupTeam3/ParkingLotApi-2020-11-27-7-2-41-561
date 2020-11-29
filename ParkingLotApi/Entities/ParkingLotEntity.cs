using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using ParkingLotApi.Dtos;

namespace ParkingLotApi.Entities
{
    public class ParkingLotEntity
    {
        public ParkingLotEntity()
        {
            var a = new OrderEntity();
        }

        public ParkingLotEntity(ParkingLot parkingLot)
        {
            Name = parkingLot.Name;
            Capacity = parkingLot.Capacity;
            Location = parkingLot.Location;
            Orders = new List<OrderEntity>();
        }

        [Key]
        public string Name { get; set; }
        public int Capacity { get; set; }
        public string Location { get; set; }
        [ForeignKey("ParkingLotName")]
        public List<OrderEntity> Orders { get; set; }
    }
}
