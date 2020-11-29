using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;
using ParkingLotApi.Dtos;

namespace ParkingLotApi.Entities
{
    public class ParkingOrderEntity
    {
        public ParkingOrderEntity()
        {
        }

        public ParkingOrderEntity(ParkingOrderDto parkingOrderDto)
        {
            this.OrderNumber = parkingOrderDto.OrderNumber;
            this.ParkingLotName = parkingOrderDto.ParkingLotName;
            this.PlateNumber = parkingOrderDto.PlateNumber;
            this.OrderStatus = parkingOrderDto.OrderStatus;
        }

        [Key]
        public string OrderNumber { get; set; }
        [ForeignKey("ParkingLotName")]
        public string ParkingLotName { get; set; }
        public string PlateNumber { get; set; }
        public DateTime CreateTime { get; set; }
        public DateTime CloseTime { get; set; }
        public string OrderStatus { get; set; }
    }
}
