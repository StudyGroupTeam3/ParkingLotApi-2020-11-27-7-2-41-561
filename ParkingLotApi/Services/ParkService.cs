using ParkingLotApi.Dtos;
using ParkingLotApi.Entities;
using ParkingLotApi.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ParkingLotApi.Services
{
    public class ParkService
    {
        private readonly ParkingLotContext parkingLotContext;

        public ParkService(ParkingLotContext parkingLotContext)
        {
            this.parkingLotContext = parkingLotContext;
        }

        public ParkingLotEntity GetParkingLotByName(string parkingLotName)
        {
            return parkingLotContext.ParkingLots.FirstOrDefault(parkingLotEntity => parkingLotEntity.Name == parkingLotName);
        }

        public bool IsFreeSpaceInParkingLot(string parkingLotName)
        {
            var parkingLot = GetParkingLotByName(parkingLotName);
            if (parkingLot is null)
            {
                return false;
            }

            var parkingLotCapacity = parkingLot.Capacity;
            if (parkingLotContext.ParkingOrders.Where(parkingOrder => parkingOrder.NameOfParkingLot == parkingLotName && parkingOrder.OrderStatus == true).Count() < parkingLotCapacity)
            {
                return true;
            }

            return false;
        }

        public async Task<ParkingOrderDto> ParkCar(ParkingOrderDto parkingOrderDto)
        {
            var parkingOrderEntity = new ParkingOrderEntity(parkingOrderDto);
            await parkingLotContext.ParkingOrders.AddAsync(parkingOrderEntity);
            await parkingLotContext.SaveChangesAsync();
            return new ParkingOrderDto(parkingOrderEntity);
        }
    }
}
