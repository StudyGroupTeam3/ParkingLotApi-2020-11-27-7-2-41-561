using Microsoft.EntityFrameworkCore;
using ParkingLotApi.Dtos;
using ParkingLotApi.Entities;
using ParkingLotApi.Repository;
using System.Collections.Generic;
using System.Threading.Tasks;
using ParkingLotApi.Models;

namespace ParkingLotApi.Services
{
    public class OrderService
    {
        private readonly ParkingLotContext context;
        public OrderService(ParkingLotContext context)
        {
            this.context = context;
        }

        public async Task<OrderEntity> AddOrder(OrderRequest order)
        {
            var orderEntity = await context.Orders.AddAsync(new OrderEntity(order));
            await context.SaveChangesAsync();

            return orderEntity.Entity;
        }

        public async Task<OrderEntity> AddOrder<T>(OrderRequest order)
        {
            var orderEntity = await context.Orders.AddAsync(new OrderEntity(order));
            await context.SaveChangesAsync();

            return orderEntity.Entity;
        }

        public async Task<List<OrderEntity>> GetOrders()
        {
            var orderEntities = await context.Orders.ToListAsync();

            return orderEntities;
        }

        public async Task<Order> GetOrderByNumber(int number)
        {
            var orderEntity = await context.Orders.FirstOrDefaultAsync(order => order.OrderNumber == number);

            return new Order(orderEntity);
        }

        public async Task UpdateOrder(int number, OrderUpdateModel data)
        {
            var orderEntity = context.Orders.FirstOrDefaultAsync(order => order.OrderNumber == number).Result;
            orderEntity.Status = data.Status;
            orderEntity.CloseTime = data.CloseTime;
            await context.SaveChangesAsync();
        }
    }
}
