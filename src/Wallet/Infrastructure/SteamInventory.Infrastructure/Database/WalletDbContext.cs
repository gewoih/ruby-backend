using Microsoft.EntityFrameworkCore;
using Wallet.Domain.Models;

namespace Wallet.Infrastructure.Database
{
    public class WalletDbContext : DbContext
    {
        public DbSet<WaxpeerPayment> WaxpeerPayments { get; set; }
        
        public WalletDbContext(DbContextOptions<WalletDbContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            
            modelBuilder.Entity<Payment>()
                .HasDiscriminator<string>("Type")
                .HasValue<WaxpeerPayment>("Waxpeer");
        }
    }
}