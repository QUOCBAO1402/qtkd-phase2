namespace QTKD.Api.Models;

public class Customer
{
    public int Id { get; set; }
    public string CustomerCode { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public string TaxCode { get; set; } = string.Empty;
    public string Address { get; set; } = string.Empty;
    public string Representative { get; set; } = string.Empty;
    public string ContactInfo { get; set; } = string.Empty;
    public string Status { get; set; } = CustomerStatuses.Active;
    public DateTime CreatedAt { get; set; }

    public ICollection<Contract> Contracts { get; set; } = new List<Contract>();
    public ICollection<PriceList> PriceLists { get; set; } = new List<PriceList>();
}
