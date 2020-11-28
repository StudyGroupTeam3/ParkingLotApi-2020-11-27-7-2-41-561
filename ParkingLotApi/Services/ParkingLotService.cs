using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using ParkingLotApi.Dtos;
using ParkingLotApi.Repository;
using System.Threading.Tasks;
using ParkingLotApi.Entities;

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

        public async Task<IEnumerable<ParkingLot>> GetAllParkingLots()
        {
            var parkingLotEntities = await context.ParkingLots.ToListAsync();

            return parkingLotEntities.Select(lot => new ParkingLot(lot)).ToList();
        }
    }
}
