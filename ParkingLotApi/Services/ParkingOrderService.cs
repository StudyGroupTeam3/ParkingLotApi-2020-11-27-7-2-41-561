using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ParkingLotApi.Dtos;
using ParkingLotApi.Entities;
using ParkingLotApi.Repository;

namespace ParkingLotApi.Services
{
    public class ParkingOrderService
    {
        private readonly ParkingLotContext parkingOrderContext;

        public ParkingOrderService(ParkingLotContext parkingOrderContext)
        {
            this.parkingOrderContext = parkingOrderContext;
        }

        public async Task<int> AddParkingOrder(ParkingOrderDto parkingOrderDto)
        {
            ParkingOrderEntity parkingOrderEntity = new ParkingOrderEntity(parkingOrderDto);
            await this.parkingOrderContext.ParkingOrder.AddAsync(parkingOrderEntity);
            await this.parkingOrderContext.SaveChangesAsync();
            return parkingOrderEntity.Id;
        }

        public async Task<ParkingOrderDto> GetOrderById(int id)
        {
            var foundParkingOrderEntity = await this.parkingOrderContext.ParkingOrder.FirstOrDefaultAsync(parkingOrderEntity => parkingOrderEntity.Id == id);
            return new ParkingOrderDto(foundParkingOrderEntity);
        }
    }
}
