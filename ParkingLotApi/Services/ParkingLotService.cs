using Microsoft.EntityFrameworkCore;
using ParkingLotApi.Dtos;
using ParkingLotApi.Entities;
using ParkingLotApi.Models;
using ParkingLotApi.Repository;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ParkingLotApi.Services
{
    public class ParkingLotService
    {
        private readonly ParkingLotContext context;

        public ParkingLotService(ParkingLotContext context)
        {
            this.context = context;
        }

        public async Task<int> AddParkingLot(ParkingLot parkingLot)
        {
            var parkingLotEntity = await context.ParkingLots.AddAsync(new ParkingLotEntity(parkingLot));
            await context.SaveChangesAsync();

            return parkingLotEntity.Entity.Id;
        }

        public async Task<ParkingLot> GetParkingLotById(int id)
        {
            var parkingLotEntityFound = await context.ParkingLots.FirstOrDefaultAsync(lot => lot.Id == id);

            return parkingLotEntityFound == null ? null : new ParkingLot(parkingLotEntityFound);
        }

        public async Task DeleteParkingLot(int id)
        {
            var parkingLotEntity = context.ParkingLots.FirstOrDefaultAsync(lot => lot.Id == id).Result;
            context.ParkingLots.Remove(parkingLotEntity);
            await context.SaveChangesAsync();
        }

        public async Task<List<ParkingLot>> GetAllParkingLots()
        {
            var parkingLotEntities = await context.ParkingLots.ToListAsync();

            return parkingLotEntities.Select(lot => new ParkingLot(lot)).ToList();
        }

        public async Task UpdateParkingLot(int id, ParkingLotUpdateModel data)
        {
            var parkingLotEntity = context.ParkingLots.FirstOrDefaultAsync(lot => lot.Id == id).Result;
            parkingLotEntity.Capacity = data.Capacity;
            await context.SaveChangesAsync();
        }
    }
}
