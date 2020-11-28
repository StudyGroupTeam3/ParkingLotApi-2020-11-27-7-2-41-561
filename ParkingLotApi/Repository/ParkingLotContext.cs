using Microsoft.EntityFrameworkCore;
using ParkingLotApi.Entities;

namespace ParkingLotApi.Repository
{
    public class ParkingLotContext : DbContext
    {
        public ParkingLotContext(DbContextOptions<ParkingLotContext> options)
            : base(options)
        {
        }

        public DbSet<ParkingLotEntity> ParkingLots { get; set; }
    }
}