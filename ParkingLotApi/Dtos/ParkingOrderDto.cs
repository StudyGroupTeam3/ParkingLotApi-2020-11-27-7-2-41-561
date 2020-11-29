using ParkingLotApi.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ParkingLotApi.Dtos
{
    public class ParkingOrderDto
    {
        public ParkingOrderDto()
        {
        }

        public ParkingOrderDto(ParkingOrderEntity parkingOrderEntity)
        {
            this.OrderNumber = parkingOrderEntity.OrderNumber;
            this.NameOfParkingLot = parkingOrderEntity.NameOfParkingLot;
            this.PlateNumber = parkingOrderEntity.PlateNumber;
            this.CreationTime = parkingOrderEntity.CreationTime;
        }

        public int OrderNumber { get; set; }
        [Required]
        public string NameOfParkingLot { get; set; }
        [Required]
        public string PlateNumber { get; set; }
        public DateTime CreationTime { get; set; }
    }
}
