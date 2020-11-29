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
    public class ParkingLotService
    {
        private ParkingLotContext parkingLotContext;

        public ParkingLotService(ParkingLotContext parkingLotContext)
        {
            this.parkingLotContext = parkingLotContext;
        }

        public async Task<ParkingLotDto> GetParkingLotByName(string name)
        {
            var foundParkingLot = await this.parkingLotContext.ParkingLots.FirstOrDefaultAsync(parkingLotEntity => parkingLotEntity.Name == name);
            return new ParkingLotDto(foundParkingLot);
        }

        public async Task<List<ParkingLotDto>> GetParkingLotsByPage(int page)
        {
            const int pageSize = 15;
            var parkingLotEntities = await parkingLotContext.ParkingLots.ToListAsync();
            var lotLists = parkingLotEntities.Select((parkingLotEntity, index) => new { Index = index, Value = parkingLotEntity })
                .GroupBy(x => x.Index / pageSize)
                .Select(x => x.Select(y => y.Value).ToList())
                .ToList();
            return lotLists[page - 1].Select(lot => new ParkingLotDto(lot)).ToList();
        }

        public async Task<string> AddParkingLot(ParkingLotDto parkingLotDto)
        {
            ParkingLotEntity parkingLotEntity = new ParkingLotEntity(parkingLotDto);
            var foundParkingLot = await GetParkingLotByName(parkingLotDto.Name);
            if (foundParkingLot.Name == null)
            {
                await this.parkingLotContext.ParkingLots.AddAsync(parkingLotEntity);
                await this.parkingLotContext.SaveChangesAsync();
                return parkingLotEntity.Name;
            }

            return null;
        }

        public async Task<ParkingLotDto> UpdateParkingLotCapacity(string parkingLotName, UpdateParkingLotCapacityDto updateParkingLotCapacityDto)
        {
            var foundParkingLot = await this.parkingLotContext.ParkingLots.FirstOrDefaultAsync(parkingLotEntity => parkingLotEntity.Name == parkingLotName);
            if (foundParkingLot != null)
            {
                foundParkingLot.Capacity = updateParkingLotCapacityDto.Capacity;
                this.parkingLotContext.ParkingLots.Update(foundParkingLot);
                await this.parkingLotContext.SaveChangesAsync();
                return new ParkingLotDto(foundParkingLot);
            }

            return null;
        }

        public async Task DeleteParkingLot(string name)
        {
            var foundParkingLot = await parkingLotContext.ParkingLots.FirstOrDefaultAsync(parkingLotEntity => parkingLotEntity.Name == name);
            if (foundParkingLot != null)
            {
                this.parkingLotContext.ParkingLots.Remove(foundParkingLot);
                await this.parkingLotContext.SaveChangesAsync();
            }
        }
    }
}
