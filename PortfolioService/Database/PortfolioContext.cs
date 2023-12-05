using System;
using System.Reflection;
using Microsoft.EntityFrameworkCore;
using SALearning.DBModel;

namespace SALearning.Database
{
    public class PortfolioContext : DbContext
    {
        public PortfolioContext(DbContextOptions<PortfolioContext> options) : base(options)
        {}
        
        // Primary Tables
        public DbSet<DBProfile> Profiles {get; set; }
        public DbSet<DBAccount> Accounts {get; set; }
        public DbSet<DBHolding> Holdings {get; set; }
        public DbSet<DBOperation> Operations {get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<DBAccount>()
                .ToTable("account")
                .HasKey(nameof(DBAccount.AccountNumber));
            
            modelBuilder.Entity<DBAccount>()
                .Property(e => e.AccountNumber)
                .ValueGeneratedOnAdd();

            modelBuilder.Entity<DBAccount>()
                .Property(e => e.ProfileId)
                .HasColumnName("profile_id");

            modelBuilder.Entity<DBHolding>()
                .ToTable("holding")
                .HasKey(nameof(DBHolding.AccountNumber), nameof(DBHolding.Symbol));

            modelBuilder.Entity<DBOperation>()
                .ToTable("operation")
                .HasKey(nameof(DBOperation.OperationId));

            modelBuilder.Entity<DBOperation>()
                .Property(e => e.OperationId)
                .ValueGeneratedOnAdd();

            modelBuilder.Entity<DBProfile>()
                .ToTable("profile")
                .HasKey(nameof(DBProfile.ProfileId));
            
            modelBuilder.Entity<DBProfile>()
                .Property(e => e.ProfileId)
                .HasColumnName("profile_id")
                .ValueGeneratedOnAdd();
                
            modelBuilder.Entity<DBProfile>()
                .Property(e => e.AccountType)
                .HasConversion(
                    v => v.ToString(),
                    v => (SALearning.ApiModel.IdentityType)Enum.Parse(typeof(SALearning.ApiModel.IdentityType), v)
                );
        }
    }
}
