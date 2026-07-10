namespace BranchService.Contracts.Events.CompanyServiceEvents;

public class CompanyServiceDeletedEvent
{
    public DateTimeOffset OccuredAt { get; set; }
    public int CompanyId { get; set; }
    public int BranchId { get; set; }
    public int CompanyServiceId { get; set; }
    public string ServiceName { get; set; }
    public string ServiceDescription { get; set; }
    public int ServiceDuration { get; set; }
    public AuditData? AuditData { get; set; }
}