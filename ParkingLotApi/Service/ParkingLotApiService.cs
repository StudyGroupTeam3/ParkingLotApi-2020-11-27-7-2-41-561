using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using ParkingLotApi.Data_Entity;
using ParkingLotApi.DTO;
using ParkingLotApi.Repository;

namespace ParkingLotApi.Service
{
    public class ParkingLotApiService
    {
        private readonly ParkingLotContext parkingLotDbContext;
        private int pageSize = 15;

        public ParkingLotApiService(ParkingLotContext parkingLotDbContext)
        {
            this.parkingLotDbContext = parkingLotDbContext;
        }

        public async Task<int> AddParkingLotAsnyc(ParkinglotDTO parkinglotDto)
        {
            if (await parkingLotDbContext.Parkinglots.FirstOrDefaultAsync(parkinglot => parkinglot.Name == parkinglotDto.Name) != null)
            {
                return -1;
            }

            ParkinglotEntity parkinglot = new ParkinglotEntity(parkinglotDto);
            await parkingLotDbContext.Parkinglots.AddAsync(parkinglot);
            await parkingLotDbContext.SaveChangesAsync();
            return parkinglot.ID;
        }

        public async Task<ParkinglotDTO> GetById(int parkinglotId)
        {
            ParkinglotEntity parkingLot = await parkingLotDbContext.Parkinglots
                .FirstOrDefaultAsync(parkinglotEntity => parkinglotEntity.ID == parkinglotId);
            return new ParkinglotDTO(parkingLot);
        }

        public async Task<List<ParkinglotDTO>> GetAll()
        { 
            var parkingLots = await parkingLotDbContext.Parkinglots.ToListAsync();
            return parkingLots.Select(parkingLot => new ParkinglotDTO(parkingLot)).ToList();
        }

        public async Task<List<ParkinglotDTO>> GetByPage(int startPage)
        {
            var parkingLots = await parkingLotDbContext.Parkinglots.ToListAsync();
            var selectedPage = parkingLots.Select(parkingLot => parkingLot).Skip((startPage - 1) * pageSize).Take(pageSize).ToList();
            return selectedPage.Select(parkingLot => new ParkinglotDTO(parkingLot)).ToList();
        }

        public async Task<bool> DeleteById(int id)
        {
            var foundParkingLot = await this.parkingLotDbContext.Parkinglots.FirstOrDefaultAsync(parkingLot => parkingLot.ID == id);
            if (foundParkingLot == null)
            {
                return false;
            }

            this.parkingLotDbContext.Parkinglots.Remove(foundParkingLot);
            await this.parkingLotDbContext.SaveChangesAsync();
            return true;
        }

        public async Task<ParkinglotDTO> GetByName(string name)
        {
            var foundParkingLot = await this.parkingLotDbContext.Parkinglots.FirstOrDefaultAsync
                (parkingLot => parkingLot.Name == name);
            return new ParkinglotDTO(foundParkingLot);
        }

        public async Task<int> ChangeCapacity(UpdateModel updateModel)
        {
            var parkingLots = await parkingLotDbContext.Parkinglots.ToListAsync();
            var targetParkinglot = parkingLots.FirstOrDefault(parkingLot => parkingLot.Name == updateModel.Name);
            targetParkinglot.Capacity = updateModel.Capacity;
            await parkingLotDbContext.SaveChangesAsync();
            return targetParkinglot.Capacity;
        }

        public async Task<OrderDto> GetOrderById(int id)
        {
            var orders = await parkingLotDbContext.Orders.ToListAsync();
            var targetOrder = orders.FirstOrDefault(order => order.Id == id);
            return new OrderDto(targetOrder);
        }

        public async Task<int> CreateOrder(OrderDto newOrderDto)
        {
            var parkingLots = await parkingLotDbContext.Parkinglots.
                Include(parklot => parklot.Orders).ToListAsync();
            var targetParkinglot = parkingLots.FirstOrDefault(parkingLot 
                => parkingLot.Name == newOrderDto.NameOfParkingLot);
            var orderEntity = new OrderEntity(newOrderDto);
            orderEntity.OrderStatus = "Open";
            var occupiedPositions = 0;
            foreach (var order in targetParkinglot.Orders)
            {
                if (order.OrderStatus.ToLower() == "open")
                {
                    occupiedPositions += 1;
                }
            }

            if (occupiedPositions >= targetParkinglot.Capacity)
            {
                return -1;
            }

            targetParkinglot.Orders.Add(orderEntity);
            await parkingLotDbContext.Orders.AddAsync(orderEntity);
            await parkingLotDbContext.SaveChangesAsync();
            return orderEntity.Id;
        }

        public async Task<bool> CloseOrder(OrderDto newOrderDto)
        {
            var targetOrder = parkingLotDbContext.Orders.FirstOrDefault(order =>
                order.OrderNumber == newOrderDto.OrderNumber
                && order.PlateNumber == newOrderDto.PlateNumber
                && order.NameOfParkingLot == newOrderDto.NameOfParkingLot
                && order.CreationTime == newOrderDto.CreationTime);

            if (targetOrder != null)
            {
                targetOrder.OrderStatus = "Closed";
                targetOrder.ClosedTime = DateTime.Now;
                await parkingLotDbContext.SaveChangesAsync();
                return true;
            }

            return false;
        }
    }
}
