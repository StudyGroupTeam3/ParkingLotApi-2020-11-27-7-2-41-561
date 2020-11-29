using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;
using ParkingLotApi.Entities;

namespace ParkingLotApi.Dtos
{
    public class ParkingOrderDto
    {
        public ParkingOrderDto()
        {
        }

        public ParkingOrderDto(ParkingOrderEntity parkingOrderEntity)
        {
            this.ParkingLotName = parkingOrderEntity?.ParkingLotName;
            this.PlateNumber = parkingOrderEntity?.PlateNumber;
            this.OrderStatus = parkingOrderEntity?.OrderStatus;
        }

        public string OrderNumber { get; set; } = Guid.NewGuid().ToString();
        public string ParkingLotName { get; set; }
        public string PlateNumber { get; set; }
        public DateTime CreateTime { get; set; }
        public DateTime CloseTime { get; set; }
        public string OrderStatus { get; set; } = "open";
    }
}
