using Microsoft.EntityFrameworkCore;
using ParkingLotApi.Data_Entity;

namespace ParkingLotApi.Repository
{
    public class ParkingLotContext : DbContext
    {
        public ParkingLotContext(DbContextOptions<ParkingLotContext> options)
            : base(options)
        {
        }

        public DbSet<ParkinglotEntity> Parkinglots { get; set; }
        public DbSet<OrderEntity> Orders { get; set; }
    }
}