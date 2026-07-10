using BranchService.Contracts.Events.Enums;

namespace BranchService.Contracts.Events.CompanyEvents;

public class CompanyDeletedEvent
{
    public DateTimeOffset OccuredAt { get; set; }
    public int CompanyId { get; set; }
    public string CompanyName { get; set; }
    public string Address { get; set; }
    public string PhoneNumber { get; set; }
    public string EmailAddress { get; set; }
    public CompanyCategory CompanyCategory { get; set; }
    
    public AuditData? AuditData { get; set; }
}