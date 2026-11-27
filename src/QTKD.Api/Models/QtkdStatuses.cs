namespace QTKD.Api.Models;

public static class CustomerStatuses
{
    public const string Active = "Active";
    public const string Inactive = "Inactive";
}

public static class ContractStatuses
{
    public const string Draft = "Draft";
    public const string PendingApproval = "PendingApproval";
    public const string RevisionRequested = "RevisionRequested";
    public const string Approved = "Approved";
    public const string Rejected = "Rejected";
    public const string Active = "Active";
    public const string Expired = "Expired";
    public const string Cancelled = "Cancelled";
}

public static class PriceListStatuses
{
    public const string Draft = "Draft";
    public const string Effective = "Effective";
    public const string Expired = "Expired";
    public const string Superseded = "Superseded";
}
