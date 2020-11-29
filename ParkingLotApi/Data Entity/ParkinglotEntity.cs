using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ParkingLotApi.DTO;

namespace ParkingLotApi.Data_Entity
{
    public class ParkinglotEntity
    {
        public ParkinglotEntity()
        {
        }

        public ParkinglotEntity(ParkinglotDTO parlParkinglotDto)
        {
            Name = parlParkinglotDto.Name;
            Capacity = parlParkinglotDto.Capacity;
            Location = parlParkinglotDto.Location;
        }

        public int ID { get; set; }
        public string Name { get; set; }
        public int Capacity { get; set; }
        public string Location { get; set; }
        public List<OrderEntity> Orders { get; set; }
    }
}
