using System;
using System.ComponentModel;

namespace ParkingLotApi.Dtos
{
    [DefaultValue(Open)]
    public enum Status
    {
        Open,
        Close
    }

    public class Order
    {
        public Order()
        {
        }

        public Status Status { get; set; }
        public int OrderNumber { get; set; }
        public string ParkingLotName { get; set; }
        public string PlateNumber { get; set; }
        public DateTime CreationTime { get; set; }
        public DateTime CloseTime { get; set; }
    }
}
