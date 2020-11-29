using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ParkingLotApi.DTO;

namespace ParkingLotApi.Data_Entity
{
    public class ParkinglotEntity
    {
        private List<OrderEntity> orders = new List<OrderEntity>();
        public ParkinglotEntity()
        {
        }

        public ParkinglotEntity(ParkinglotDTO parkinglotDto)
        {
            Name = parkinglotDto.Name;
            Capacity = parkinglotDto.Capacity;
            Location = parkinglotDto.Location;
        }

        public ParkinglotEntity(ParkinglotEntity parkinglot, OrderEntity order)
        {
            ID = parkinglot.ID;
            Name = parkinglot.Name;
            Capacity = parkinglot.Capacity;
            Location = parkinglot.Location;
            Orders.Add(order);
        }

        public int ID { get; set; }
        public string Name { get; set; }
        public int Capacity { get; set; }
        public string Location { get; set; }

        public List<OrderEntity> Orders
        {
            get
            {
                return orders;
            }
            set
            {
                orders = value;
            }
        }
    }
}
