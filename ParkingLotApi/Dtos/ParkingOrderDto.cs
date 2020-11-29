using System;
using System.Collections.Generic;
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
            this.ParkingLotName = parkingOrderEntity.ParkingLotName;
            this.PlateNumber = parkingOrderEntity.PlateNumber;
            this.CreateTime = parkingOrderEntity.CreateTime;
            this.CloseTime = parkingOrderEntity.CloseTime;
            this.OrderStatus = parkingOrderEntity.OrderStatus;
        }

        public string OrderNumber { get; set; }
        public string ParkingLotName { get; set; }
        public string PlateNumber { get; set; }
        public DateTime CreateTime { get; set; } = DateTime.Now;
        public DateTime CloseTime { get; set; } = DateTime.Now;
        public bool OrderStatus { get; set; } = false;
    }
}
