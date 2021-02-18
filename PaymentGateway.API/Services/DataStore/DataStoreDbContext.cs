using Microsoft.EntityFrameworkCore;

namespace PaymentGateway.Services.DataStore
{
    public class DataStoreDbContext : DbContext
    {
        public DataStoreDbContext(DbContextOptions<DataStoreDbContext> options) : base(options)
        {
        }
        public DbSet<Payment> Payments { get; set; }
    }
}