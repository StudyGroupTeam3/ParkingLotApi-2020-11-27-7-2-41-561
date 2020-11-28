using ParkingLotApi.Entities;

namespace ParkingLotApi.Dtos
{
    public class ParkingLot
    {
        public ParkingLot()
        {
        }

        public ParkingLot(string name, int capacity, string location)
        {
            Name = name;
            Capacity = capacity;
            Location = location;
        }

        public ParkingLot(ParkingLotEntity parkingLot)
        {
            Name = parkingLot.Name;
            Capacity = parkingLot.Capacity;
            Location = parkingLot.Location;
        }

        public string Name { get; set; }
        public int Capacity { get; set; }
        public string Location { get; set; }

        public override bool Equals(object? other)
        {
            var parkingLot = (ParkingLot)other;
            return parkingLot != null &&
                   (parkingLot.Location == Location
                    && parkingLot.Name == Name
                    && parkingLot.Capacity == Capacity);
        }
    }
}
