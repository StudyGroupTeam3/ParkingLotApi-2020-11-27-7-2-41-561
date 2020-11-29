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
    public class ParkingLotService
    {
        private readonly ParkingLotContext parkingLotContext;

        public ParkingLotService(ParkingLotContext parkingLotContext)
        {
            this.parkingLotContext = parkingLotContext;
        }

        public async Task<int> AddParkingLot(ParkingLotDto parkingLotDto)
        {
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

        //public async Task<List<ParkingLotDto>> GetAll()
        //{
        //    var parkingLots = await this.parkingLotContext.ParkingLot
        //        .ToListAsync();
        //    return parkingLots.Select(companyEntity => new ParkingLotDto(companyEntity)).ToList();
        //}

        public async Task<List<ParkingLotDto>> GetAllByPages(int startPage, int pageSize)
        {
            int skip = (startPage - 1) * pageSize;

            var parkingLotsByPages = await this.parkingLotContext.ParkingLot
                .OrderBy(c => c.Id)
                .Skip(skip)
                .Take(pageSize)
                .ToListAsync();

            return parkingLotsByPages.Select(companyEntity => new ParkingLotDto(companyEntity)).ToList();
        }

        public async Task DeleteParkingLot(int id)
        {
            var foundParkingLot = await this.parkingLotContext.ParkingLot.FirstOrDefaultAsync(parkingLot => parkingLot.Id == id);
            this.parkingLotContext.ParkingLot.Remove(foundParkingLot);
            await this.parkingLotContext.SaveChangesAsync();
        }

        public async Task<ParkingLotDto> UpdateById(int id, UpdateParkingLotDto updateParkingLotDto)
        {
            var foundParkingLotEntity = await this.parkingLotContext.ParkingLot.FirstOrDefaultAsync(parkingLotEntity => parkingLotEntity.Id == id);
            foundParkingLotEntity.Capacity = updateParkingLotDto.Capacity;
            return new ParkingLotDto(foundParkingLotEntity);
        }
    }
}
