using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using ParkingLotApi.Dtos;

namespace ParkingLotApi.Entities
{
    [DefaultValue(Open)]
    public enum Status
    {
        Open,
        Close
    }

    public class OrderEntity
    {
        public OrderEntity()
        {
        }

        public OrderEntity(OrderRequest order)
        {
            ParkingLotName = order.ParkingLotName;
            PlateNumber = order.PlateNumber;
            CreationTime = DateTime.Now;
        }

        public Status Status { get; set; }
        [Key]
        public int OrderNumber { get; set; }
        [ForeignKey("ParkingLotName")]
        public string ParkingLotName { get; set; }
        public string PlateNumber { get; set; }
        public DateTime CreationTime { get; set; }
        public DateTime CloseTime { get; set; }
    }
}
