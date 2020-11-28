using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ParkingLotApi.DTO;
using ParkingLotApi.Repository;

namespace ParkingLotApi.Service
{
    public class ParkingLotApiService
    {
        private readonly ParkingLotContext parkingLotDbContext;

        public ParkingLotApiService(ParkingLotContext parkingLotDbContext) //DbContext was created in the StartUp， This service should get all DB method-
        {
            this.parkingLotDbContext = parkingLotDbContext;
        }

        public async Task<int> AddParkingLotAsnyc(ParkinglotDTO parkinglotDto)
        {
            return 1;
        }

        public async Task<ParkinglotDTO> GetById(int parkinglotId)
        {
            return new ParkinglotDTO()
            {
                Name = "SuperPark_1",
                Capacity = 10,
                Location = "WuDaoKong"
            };
        }
    }
}
