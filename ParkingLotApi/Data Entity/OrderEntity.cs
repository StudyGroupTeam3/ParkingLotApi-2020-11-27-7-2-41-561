using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ParkingLotApi.DTO;

namespace ParkingLotApi.Data_Entity
{
    public class OrderEntity
    {
        public OrderEntity()
        {
        }

        public OrderEntity(OrderDto orderDto)
        {
            OrderNumber = orderDto.OrderNumber;
            NameOfParkingLot = orderDto.NameOfParkingLot;
            PlateNumber = orderDto.PlateNumber;
            CreationTime = orderDto.CreationTime;
        }

        public int Id { get; set; }
        public Guid OrderNumber { get; set; }
        public string NameOfParkingLot { get; set; }
        public string PlateNumber { get; set; }
        public DateTime CreationTime { get; set; }
        public DateTime ClosedTime { get; set; }
        public string OrderStatus { get; set; }
    }
}
