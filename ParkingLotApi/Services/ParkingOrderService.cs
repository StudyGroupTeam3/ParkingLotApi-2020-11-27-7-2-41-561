using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using ParkingLotApi.Dtos;
using ParkingLotApi.Entities;
using ParkingLotApi.Repository;

namespace ParkingLotApi.Services
{
    public class ParkingOrderService
    {
        private readonly ParkingLotContext parkingLotContext;

        public ParkingOrderService(ParkingLotContext parkingLotContext)
        {
            this.parkingLotContext = parkingLotContext;
        }

        public async Task<string> AddParkingOrder(ParkingOrderDto parkingOrderDto)
        {
            ParkingOrderEntity parkingOrderEntity = new ParkingOrderEntity(parkingOrderDto);
            await parkingLotContext.ParkingOrders.AddAsync(parkingOrderEntity);
            await parkingLotContext.SaveChangesAsync();

            return parkingOrderEntity.OrderNumber;
        }

        public async Task<List<ParkingOrderEntity>> GetAllParkingOrders()
        {
            var parkingOrderEntities = await parkingLotContext.ParkingOrders.ToListAsync();
            return parkingOrderEntities;
        }

        public async Task<ParkingOrderDto> GetParkingOrderByOrderNumber(string orderNumber)
        {
            var parkingOrderEntity = await parkingLotContext.ParkingOrders.FirstOrDefaultAsync(parkingOrder => parkingOrder.OrderNumber == orderNumber);
            var parkingOrder = new ParkingOrderDto(parkingOrderEntity)
            {
                OrderNumber = parkingOrderEntity.OrderNumber,
            };
            return parkingOrder;
        }

        public async Task UpdateOrder(string number, UpdateParkingOrderDto data)
        {
            //var orderEntity = parkingLotContext.ParkingOrders.FirstOrDefaultAsync(order => order.OrderNumber == number).Result;
            //orderEntity.OrderStatus = data.OrderStatus;
            //orderEntity.CloseTime = data.CloseTime;
            //await parkingLotContext.SaveChangesAsync();
        }
    }
}
