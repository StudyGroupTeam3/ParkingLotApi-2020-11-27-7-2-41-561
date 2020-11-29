using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ParkingLotApi.Dtos
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
            this.CreateTime = parkingOrderDto.CreateTime;
            this.CloseTime = parkingOrderDto.CloseTime;
            this.OrderStatus = parkingOrderDto.OrderStatus;
        }

        public int Id { get; set; }
        public string OrderNumber { get; set; }
        public string ParkingLotName { get; set; }
        public string PlateNumber { get; set; }
        public DateTime CreateTime { get; set; } = DateTime.Now;
        public DateTime CloseTime { get; set; } = DateTime.Now;
        public bool OrderStatus { get; set; } = false;
    }
}
