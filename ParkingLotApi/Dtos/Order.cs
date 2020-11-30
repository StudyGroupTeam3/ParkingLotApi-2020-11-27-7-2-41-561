using System;
using ParkingLotApi.Entities;

namespace ParkingLotApi.Dtos
{
    public class Order
    {
        public Order()
        {
        }

        public Order(OrderEntity orderEntity)
        {
            OrderNumber = orderEntity.OrderNumber;
            ParkingLotName = orderEntity.ParkingLotName;
            PlateNumber = orderEntity.PlateNumber;
            CreationTime = orderEntity.CreationTime;
        }

        public int OrderNumber { get; set; }
        public string ParkingLotName { get; set; }
        public string PlateNumber { get; set; }
        public DateTime CreationTime { get; set; }

        public override bool Equals(object? other)
        {
            var order = (Order)other;
            return order != null &&
                   (order.ParkingLotName == ParkingLotName
                    && order.PlateNumber == PlateNumber
                    && order.OrderNumber == OrderNumber
                    && order.CreationTime == CreationTime);
        }
    }
}
