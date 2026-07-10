using MessagePack;

namespace BranchService.Contracts.Responses;

[MessagePackObject]
public class CompanyServiceResponse
{
    [Key(0)] public Guid RequestId { get; set; }
    [Key(1)] public int CompanyId { get; set; }
    [Key(2)] public int BranchId { get; set; }
    [Key(3)] public int CompanyServiceId { get; set; }
    [Key(4)] public bool IsValid { get; set; }
    [Key(5)] public string? ErrorMessage { get; set; }
    [Key(6)] public string? CompanyServiceName { get; set; }
    [Key(7)] public int ServiceDuration { get; set; }
}