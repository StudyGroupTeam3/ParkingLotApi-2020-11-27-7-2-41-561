﻿using System;
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
    }
}
