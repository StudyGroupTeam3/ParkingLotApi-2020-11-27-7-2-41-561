using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ParkingLotApi.DTO
{
    public class UpdateModel
    {
        public UpdateModel()
        {
        }

        public UpdateModel(string name, int capcacity)
        {
            Name = name;
            Capacity = capcacity;
        }

        public string Name { get; set; }
        public int Capacity { get; set; }
    }
}
