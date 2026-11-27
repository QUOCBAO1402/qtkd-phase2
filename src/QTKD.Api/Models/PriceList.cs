namespace QTKD.Api.Models;

public class PriceList
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public int? CustomerId { get; set; }
    public DateOnly EffectiveDate { get; set; }
    public DateOnly? EndDate { get; set; }
    public string Status { get; set; } = PriceListStatuses.Effective;
    public DateTime CreatedAt { get; set; }

    public Customer? Customer { get; set; }
    public ICollection<PriceItem> PriceItems { get; set; } = new List<PriceItem>();
}
