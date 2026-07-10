
using BranchService.Contracts.Events.Enums;
using MessagePack;

namespace BranchService.Contracts.Responses;

[MessagePackObject]
public class CompanyResponse
{
    [Key(0)] public Guid RequestId { get; set; }
    [Key(1)] public int CompanyId { get; set; }
    [Key(2)] public bool IsValid { get; set; }
    [Key(3)] public string? ErrorMessage { get; set; }
    [Key(4)] public string? CompanyName { get; set; }
    [Key(5)] public CompanyCategory? CompanyCategory { get; set; }
}