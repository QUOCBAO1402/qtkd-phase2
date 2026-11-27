using Microsoft.EntityFrameworkCore;
using QTKD.Api.Models;

namespace QTKD.Api.Data;

public class QtkdDbContext : DbContext
{
    public QtkdDbContext(DbContextOptions<QtkdDbContext> options) : base(options)
    {
    }

    public DbSet<Customer> Customers => Set<Customer>();
    public DbSet<Contract> Contracts => Set<Contract>();
    public DbSet<PriceList> PriceLists => Set<PriceList>();
    public DbSet<PriceItem> PriceItems => Set<PriceItem>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Customer>(entity =>
        {
            entity.ToTable("Customers");
            entity.HasKey(c => c.Id);

            entity.Property(c => c.CustomerCode).IsRequired().HasMaxLength(50);
            entity.Property(c => c.Name).IsRequired().HasMaxLength(200);
            entity.Property(c => c.TaxCode).HasMaxLength(50);
            entity.Property(c => c.Address).HasMaxLength(300);
            entity.Property(c => c.Representative).HasMaxLength(150);
            entity.Property(c => c.ContactInfo).HasMaxLength(200);
            entity.Property(c => c.Status).IsRequired().HasMaxLength(30);
            entity.Property(c => c.CreatedAt).HasDefaultValueSql("CURRENT_TIMESTAMP");

            entity.HasIndex(c => c.CustomerCode).IsUnique();
        });

        modelBuilder.Entity<Contract>(entity =>
        {
            entity.ToTable("Contracts");
            entity.HasKey(c => c.Id);

            entity.Property(c => c.ContractNo).IsRequired().HasMaxLength(50);
            entity.Property(c => c.Value).HasPrecision(18, 2);
            entity.Property(c => c.PaymentTerms).HasMaxLength(1000);
            entity.Property(c => c.ServiceTerms).HasMaxLength(2000);
            entity.Property(c => c.AttachmentUrl).HasMaxLength(500);
            entity.Property(c => c.Status).IsRequired().HasMaxLength(30);
            entity.Property(c => c.CreatedAt).HasDefaultValueSql("CURRENT_TIMESTAMP");

            entity.HasIndex(c => c.ContractNo).IsUnique();

            entity.HasOne(c => c.Customer)
                .WithMany(customer => customer.Contracts)
                .HasForeignKey(c => c.CustomerId)
                .OnDelete(DeleteBehavior.Restrict);
        });

        modelBuilder.Entity<PriceList>(entity =>
        {
            entity.ToTable("PriceLists");
            entity.HasKey(p => p.Id);

            entity.Property(p => p.Name).IsRequired().HasMaxLength(200);
            entity.Property(p => p.Status).IsRequired().HasMaxLength(30);
            entity.Property(p => p.CreatedAt).HasDefaultValueSql("CURRENT_TIMESTAMP");

            entity.HasOne(p => p.Customer)
                .WithMany(customer => customer.PriceLists)
                .HasForeignKey(p => p.CustomerId)
                .OnDelete(DeleteBehavior.SetNull);
        });

        modelBuilder.Entity<PriceItem>(entity =>
        {
            entity.ToTable("PriceItems");
            entity.HasKey(i => i.Id);

            entity.Property(i => i.ServiceCode).IsRequired().HasMaxLength(50);
            entity.Property(i => i.ServiceName).IsRequired().HasMaxLength(200);
            entity.Property(i => i.Unit).IsRequired().HasMaxLength(50);
            entity.Property(i => i.UnitPrice).HasPrecision(18, 2);

            entity.HasIndex(i => new { i.PriceListId, i.ServiceCode }).IsUnique();

            entity.HasOne(i => i.PriceList)
                .WithMany(priceList => priceList.PriceItems)
                .HasForeignKey(i => i.PriceListId)
                .OnDelete(DeleteBehavior.Cascade);
        });
    }
}
