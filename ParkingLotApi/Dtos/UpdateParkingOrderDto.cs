using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ParkingLotApi.Dtos
{
    public class UpdateParkingOrderDto
    {
        public UpdateParkingOrderDto()
        {
        }

        public UpdateParkingOrderDto(string status)
        {
            this.OrderStatus = status;
        }

        public DateTime CloseTime { get; set; }
        public string OrderStatus { get; set; }
    }
}
