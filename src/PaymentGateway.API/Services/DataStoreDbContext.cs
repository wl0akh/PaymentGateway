using Microsoft.EntityFrameworkCore;
using PaymentGateway.Domain.Entities;

namespace PaymentGateway.Services
{
    /// <summary>
    /// DataStoreDbContext class to Setup DB and DbContext in EF Core
    /// </summary>
    public class DataStoreDbContext : DbContext
    {
        public DataStoreDbContext(DbContextOptions<DataStoreDbContext> options) : base(options)
        {
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Payment>().Property(b => b.PaymentId).HasField("_paymentId");
            modelBuilder.Entity<Payment>().Property(b => b.CardNumber).HasField("_cardNumber");
            modelBuilder.Entity<Payment>().Property(b => b.PaymentStatus).HasField("_paymentStatus");
            modelBuilder.Entity<Payment>().Property(b => b.Expiry).HasField("_expiry");
            modelBuilder.Entity<Payment>().Property(b => b.Amount).HasField("_amount");
            modelBuilder.Entity<Payment>().Property(b => b.Currency).HasField("_currency");
            modelBuilder.Entity<Payment>().Property(b => b.Amount).HasField("_amount");
        }

        /// <summary>
        /// DB Entity for Payments
        /// 
        /// </summary>
        /// <value></value>
        public DbSet<Payment> Payments { get; set; }
    }
}