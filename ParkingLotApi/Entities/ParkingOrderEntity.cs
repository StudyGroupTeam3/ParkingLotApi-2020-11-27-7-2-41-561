using ParkingLotApi.Dtos;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ParkingLotApi.Entities
{
    public class ParkingOrderEntity
    {
        public ParkingOrderEntity()
        {
        }

        public ParkingOrderEntity(ParkingOrderDto parkingDto)
        {
            this.NameOfParkingLot = parkingDto.NameOfParkingLot;
            this.PlateNumber = parkingDto.PlateNumber;
            this.CreationTime = DateTime.Now;
            this.OrderStatus = true;
        }

        [Key]
        public int OrderNumber { get; set; }
        public string NameOfParkingLot { get; set; }
        public string PlateNumber { get; set; }
        public DateTime CreationTime { get; set; }
        public DateTime CloseTime { get; set; }
        public bool OrderStatus { get; set; }
    }
}
