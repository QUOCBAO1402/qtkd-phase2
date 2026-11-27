namespace QTKD.Api.Models;

public class Contract
{
    public int Id { get; set; }
    public string ContractNo { get; set; } = string.Empty;
    public int CustomerId { get; set; }
    public DateOnly StartDate { get; set; }
    public DateOnly EndDate { get; set; }
    public decimal Value { get; set; }
    public string? PaymentTerms { get; set; }
    public string? ServiceTerms { get; set; }
    public string? AttachmentUrl { get; set; }
    public string Status { get; set; } = ContractStatuses.Draft;
    public DateTime? SubmittedAt { get; set; }
    public DateTime CreatedAt { get; set; }

    public Customer? Customer { get; set; }
}
