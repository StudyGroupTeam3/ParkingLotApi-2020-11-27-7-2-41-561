namespace ParkingLotApi.Models
{
    public class ParkingLotUpdateModel
    {
        public ParkingLotUpdateModel()
        {
        }

        public ParkingLotUpdateModel(int capacity)
        {
            Capacity = capacity;
        }

        public int Capacity { get; set; }
    }
}
