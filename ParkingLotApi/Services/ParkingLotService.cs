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

        //public async Task<string> AddParkingLot(ParkingLotDto parkingLotDto)
        //{
        //    ParkingLotEntity parkingLotEntity = new ParkingLotEntity(parkingLotDto);

        //    await parkingLotContext.ParkingLots.AddAsync(parkingLotEntity);
        //    await this.parkingLotContext.SaveChangesAsync();
        //    return parkingLotEntity.Name;
        //}

        public async Task DeleteParkingLot(string name)
        {
            var foundParkingLot = await parkingLotContext.ParkingLots.FirstOrDefaultAsync(parkingLotEntity => parkingLotEntity.Name == name);
            this.parkingLotContext.ParkingLots.Remove(foundParkingLot);
            await this.parkingLotContext.SaveChangesAsync();
        }
    }
}
