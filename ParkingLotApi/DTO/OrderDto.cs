using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ParkingLotApi.Data_Entity;

namespace ParkingLotApi.DTO
{
    public class OrderDto
    {
        public OrderDto()
        {
        }

        public OrderDto(OrderEntity orderEntity)
        {
            OrderNumber = orderEntity.OrderNumber;
            NameOfParkingLot = orderEntity.NameOfParkingLot;
            PlateNumber = orderEntity.PlateNumber;
            CreationTime = orderEntity.CreationTime;
        }

        public Guid OrderNumber { get; set; }
        public string NameOfParkingLot { get; set; }
        public string PlateNumber { get; set; }
        public DateTime CreationTime { get; set; }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj))
            {
                return false;
            }

            if (ReferenceEquals(this, obj))
            {
                return true;
            }

            if (this.GetType() != obj.GetType())
            {
                return false;
            }

            return Equals((OrderDto)obj);
        }

        private bool Equals(OrderDto other)
        {
            return OrderNumber == other.OrderNumber && NameOfParkingLot == other.NameOfParkingLot
                                                    && PlateNumber == other.PlateNumber
                                                    && CreationTime == other.CreationTime;
        }
    }
}
