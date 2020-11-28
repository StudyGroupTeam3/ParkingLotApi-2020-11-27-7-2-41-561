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
    }
}
