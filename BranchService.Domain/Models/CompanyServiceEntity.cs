namespace BranchService.Domain.Models;

public class CompanyServiceEntity
{
    public int Id { get; set; }
    public string ServiceName { get; set; } = string.Empty;
    public string ServiceDescription { get; set; } = string.Empty;
    public DateTime ServiceDuration { get; set; }
    public DateTimeOffset CreatedAt { get; set; }

    public int CompanyId { get; set; }
    public int BranchId { get; set; }

    public CompanyEntity Company { get; set; }
    public BranchEntity Branch { get; set; }
}