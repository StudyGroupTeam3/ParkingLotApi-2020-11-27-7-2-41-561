using ParkingLotApi.Entities;

namespace ParkingLotApi.Dtos
{
    public class OrderRequest
    {
        public OrderRequest()
        {
        }

        public OrderRequest(OrderEntity orderEntity)
        {
            ParkingLotName = orderEntity.ParkingLotName;
            PlateNumber = orderEntity.PlateNumber;
        }

        public OrderRequest(string parkingLotName, string plateNumber)
        {
            ParkingLotName = parkingLotName;
            PlateNumber = plateNumber;
        }

        public string ParkingLotName { get; set; }
        public string PlateNumber { get; set; }
    }
}
