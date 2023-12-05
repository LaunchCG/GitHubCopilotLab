using Microsoft.EntityFrameworkCore;
using PriceService.DBModel;
using System.Diagnostics;

namespace PriceService.Database;

public class PriceContext : DbContext
{
    public PriceContext(DbContextOptions<PriceContext> options) : base(options)
    {}
    
    // Primary Tables
    public DbSet<DBCompanyInfo> CompanyInfo {get; set; }
    public DbSet<DBPrice> Price {get; set; }
    public DbSet<DBBatchRun> BatchRun {get; set; }
    public DbSet<DBBatchDetail> BatchDetail {get; set; }

    // protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    //     => optionsBuilder.LogTo(message => Debug.WriteLine(message));

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<DBCompanyInfo>()
            .ToTable("company_info");
        modelBuilder.Entity<DBPrice>()
            .ToTable("price");
        modelBuilder.Entity<DBBatchRun>()
            .ToTable("batch_run");
        modelBuilder.Entity<DBBatchDetail>()
            .ToTable("batch_detail");
    }
}
