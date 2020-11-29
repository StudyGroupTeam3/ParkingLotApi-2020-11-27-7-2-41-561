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
        private readonly ParkingLotContext parkingLotContext;

        public ParkingLotService(ParkingLotContext parkingLotContext)
        {
            this.parkingLotContext = parkingLotContext;
        }

        public async Task<int> AddParkingLot(ParkingLotDto parkingLotDto)
        {
            //var foundParkingLotEntity = await this.parkingLotContext.ParkingLot.FirstOrDefaultAsync(parkingLotEntity => parkingLotEntity.Name == parkingLotDto.Name);
            //if (foundParkingLotEntity.)
            ParkingLotEntity parkingLotEntity = new ParkingLotEntity(parkingLotDto);
            await this.parkingLotContext.ParkingLot.AddAsync(parkingLotEntity);
            await this.parkingLotContext.SaveChangesAsync();
            return parkingLotEntity.Id;
        }

        public async Task<ParkingLotDto> GetById(int id)
        {
            var foundParkingLotEntity = await this.parkingLotContext.ParkingLot.FirstOrDefaultAsync(parkingLotEntity => parkingLotEntity.Id == id);
            return new ParkingLotDto(foundParkingLotEntity);
        }
    }
}
