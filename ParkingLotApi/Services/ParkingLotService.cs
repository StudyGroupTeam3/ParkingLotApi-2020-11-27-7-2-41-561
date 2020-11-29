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

        public async Task<int> AddParkingLot(ParkingLotDto parkingLotDto)
        {
            var parkingLotEntity = new ParkingLotEntity(parkingLotDto);
            await parkingLotContext.ParkingLots.AddAsync(parkingLotEntity);
            await parkingLotContext.SaveChangesAsync();
            return parkingLotEntity.Id;
        }

        public async Task<List<ParkingLotDto>> GetParkingLotsByPage(int pageSize, int pageIndex)
        {
            var parkingLotEntities = await parkingLotContext.ParkingLots.Skip(pageSize * (pageIndex - 1)).Take(pageSize).ToListAsync();
            return parkingLotEntities.Select(parkingLotEntity => new ParkingLotDto(parkingLotEntity)).ToList();
        }

        public async Task<ParkingLotDto> GetParkingLotById(int parkingLotId)
        {
            var parkingLotEntity = await parkingLotContext.ParkingLots.FindAsync(parkingLotId);
            return parkingLotEntity is null ? null : new ParkingLotDto(parkingLotEntity);
        }

        public async Task<ParkingLotDto> DeleteParkingLotById(int parkingLotId)
        {
            var parkingLotEntity = await parkingLotContext.ParkingLots.FindAsync(parkingLotId);
            if (parkingLotEntity is null)
            {
                return null;
            }

            parkingLotContext.ParkingLots.Remove(parkingLotEntity);
            return new ParkingLotDto(parkingLotEntity);
        }
    }
}
