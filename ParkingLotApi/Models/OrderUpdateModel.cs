using ParkingLotApi.Entities;
using System;

namespace ParkingLotApi.Models
{
    public class OrderUpdateModel
    {
        public OrderUpdateModel()
        {
        }

        public OrderUpdateModel(DateTime leaveTime, Status status)
        {
            CloseTime = leaveTime;
            Status = status;
        }

        public Status Status { get; set; }
        public DateTime CloseTime { get; set; }
    }
}
