using ParkingLotApi.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ParkingLotApi.Dtos
{
    public class ParkingLotDto
    {
        public ParkingLotDto()
        {
        }

        public ParkingLotDto(ParkingLotEntity parkingLotEntity)
        {
            this.Name = parkingLotEntity.Name;
            this.Capacity = parkingLotEntity.Capacity;
            this.Location = parkingLotEntity.Location;
        }

        [Required]
        public string Name { get; set; }
        [Required(ErrorMessage = "out or range")]
        [Range(0, int.MaxValue, ErrorMessage ="out or range")]
        public int Capacity { get; set; }
        [Required]
        public string Location { get; set; }

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

            return Equals((ParkingLotDto)obj);
        }

        private bool Equals(ParkingLotDto other)
        {
            return Name == other.Name && Capacity == other.Capacity && Location == other.Location;
        }
    }

    public class ParkingLotCapacityUpdateDto
    {
        [Required]
        [Range(0, int.MaxValue)]
        public int Capacity { get; set; }
    }
}
