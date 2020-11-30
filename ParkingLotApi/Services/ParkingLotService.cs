using Microsoft.EntityFrameworkCore;
using ParkingLotApi.Dtos;
using ParkingLotApi.Entities;
using ParkingLotApi.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ParkingLotApi.Services
{
    public class ParkingLotService
    {
        private readonly ParkingLotContext parkingLotContext;

        public ParkingLotService(ParkingLotContext parkingLotContext)
        {
            this.parkingLotContext = parkingLotContext;
        }

        public async Task<string> AddParkingLot(ParkingLotDto parkingLotDto)
        {
            var parkingLotEntity = new ParkingLotEntity(parkingLotDto);
            await parkingLotContext.ParkingLots.AddAsync(parkingLotEntity);
            await parkingLotContext.SaveChangesAsync();
            return parkingLotEntity.Name;
        }

        public async Task<List<ParkingLotDto>> GetParkingLotsByPage(int pageSize, int pageIndex)
        {
            var parkingLotEntities = await parkingLotContext.ParkingLots.Skip(pageSize * (pageIndex - 1)).Take(pageSize).ToListAsync();
            return parkingLotEntities.Select(parkingLotEntity => new ParkingLotDto(parkingLotEntity)).ToList();
        }

        public async Task<ParkingLotDto> GetParkingLotByName(string parkingLotName)
        {
            var parkingLotEntity = await parkingLotContext.ParkingLots.FirstOrDefaultAsync(parkingLot => parkingLot.Name == parkingLotName);
            return parkingLotEntity is null ? null : new ParkingLotDto(parkingLotEntity);
        }

        public async Task<ParkingLotDto> DeleteParkingLotByName(string parkingLotName)
        {
            var parkingLotEntity = await parkingLotContext.ParkingLots.FirstOrDefaultAsync(parkingLot => parkingLot.Name == parkingLotName);
            if (parkingLotEntity is null)
            {
                return null;
            }

            parkingLotContext.ParkingLots.Remove(parkingLotEntity);
            await parkingLotContext.SaveChangesAsync();
            return new ParkingLotDto(parkingLotEntity);
        }

        public async Task<ParkingLotDto> UpdateParkingLotCapacityByName(string parkingLotName, ParkingLotCapacityUpdateDto parkingLotCapacityUpdateModel)
        {
            var parkingLotEntity = await parkingLotContext.ParkingLots.FirstOrDefaultAsync(parkingLot => parkingLot.Name == parkingLotName);
            if (parkingLotEntity is null)
            {
                return null;
            }

            parkingLotEntity.Capacity = parkingLotCapacityUpdateModel.Capacity;
            parkingLotContext.SaveChanges();
            return new ParkingLotDto(parkingLotEntity);
        }

        public bool IsParkingLotNameExisting(string name)
        {
            if (parkingLotContext.ParkingLots.FirstOrDefault(parkingLot => parkingLot.Name == name) is null)
            {
                return false;
            }

            return true;
        }
    }
}
