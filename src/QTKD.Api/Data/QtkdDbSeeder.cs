using Microsoft.EntityFrameworkCore;
using QTKD.Api.Models;

namespace QTKD.Api.Data;

public static class QtkdDbSeeder
{
    public static async Task SeedAsync(QtkdDbContext db)
    {
        if (await db.Customers.AnyAsync())
        {
            return;
        }

        Customer minhPhat = new()
        {
            CustomerCode = "KH-MP01",
            Name = "Công ty Minh Phát",
            TaxCode = "0312456789",
            Address = "120 Nguyễn Văn Linh, Quận 7, TP.HCM",
            Representative = "Nguyễn Minh Phát",
            ContactInfo = "minhphat@minhphatlogistics.vn - 0909123456",
            Status = CustomerStatuses.Active
        };

        Customer haiDang = new()
        {
            CustomerCode = "KH-HD02",
            Name = "Công ty TNHH Vận tải Hải Đăng",
            TaxCode = "0319876543",
            Address = "45 Hoàng Diệu, Quận 4, TP.HCM",
            Representative = "Trần Hải Đăng",
            ContactInfo = "contact@haidangport.vn - 0918654321",
            Status = CustomerStatuses.Active
        };

        Contract sampleContract = new()
        {
            ContractNo = "HD-2026-018",
            Customer = minhPhat,
            StartDate = new DateOnly(2026, 12, 1),
            EndDate = new DateOnly(2027, 11, 30),
            Value = 450_000_000m,
            PaymentTerms = "Thanh toán chuyển khoản trong vòng 15 ngày kể từ ngày xuất hóa đơn.",
            ServiceTerms = "Cung cấp dịch vụ bốc xếp container và lưu kho hàng hóa tại cảng ABC.",
            AttachmentUrl = null,
            Status = ContractStatuses.Draft
        };

        PriceList samplePriceList = new()
        {
            Name = "Bảng giá dịch vụ cảng & lưu kho 2026 - Minh Phát",
            Customer = minhPhat,
            EffectiveDate = new DateOnly(2026, 12, 1),
            EndDate = new DateOnly(2027, 11, 30),
            Status = PriceListStatuses.Effective,
            PriceItems = new List<PriceItem>
            {
                new()
                {
                    ServiceCode = "BX-CONT20",
                    ServiceName = "Bốc xếp container 20 feet",
                    Unit = "Container",
                    UnitPrice = 1_250_000m
                },
                new()
                {
                    ServiceCode = "LK-KHO",
                    ServiceName = "Lưu kho hàng hóa tiêu chuẩn",
                    Unit = "Tấn/ngày",
                    UnitPrice = 85_000m
                }
            }
        };

        db.Customers.AddRange(minhPhat, haiDang);
        db.Contracts.Add(sampleContract);
        db.PriceLists.Add(samplePriceList);

        await db.SaveChangesAsync();
    }
}
