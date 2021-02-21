using Microsoft.EntityFrameworkCore;

namespace PaymentGateway.Services.DataStore
{
    /// <summary>
    /// DataStoreDbContext class to Setup DB and DbContext in EF Core
    /// </summary>
    public class DataStoreDbContext : DbContext
    {
        public DataStoreDbContext(DbContextOptions<DataStoreDbContext> options) : base(options)
        {
        }

        /// <summary>
        /// DB Entity for Payments
        /// </summary>
        /// <value></value>
        public DbSet<Payment> Payments { get; set; }
    }
}