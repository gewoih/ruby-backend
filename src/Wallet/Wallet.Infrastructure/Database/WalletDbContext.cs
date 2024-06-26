﻿using Microsoft.EntityFrameworkCore;
using Wallet.Domain.Models;
using Wallet.Domain.Models.Payments;
using Wallet.Domain.Models.Payments.NowPayments;
using Wallet.Domain.Models.Payments.Waxpeer;

namespace Wallet.Infrastructure.Database
{
	public class WalletDbContext : DbContext
    {
        public DbSet<WaxpeerPayment> WaxpeerPayments { get; set; }
        public DbSet<NowPayment> NowPayments { get; set; }
        public DbSet<Promocode> Promocodes { get; set; }
        
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