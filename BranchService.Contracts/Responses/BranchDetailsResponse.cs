using BranchService.Contracts.Events.Enums;
using MessagePack;

namespace BranchService.Contracts.Responses;

[MessagePackObject]
public class BranchDetailsResponse
{
    [Key(0)] public int BranchId { get; set; }
    [Key(1)] public int CompanyId { get; set; }

    [Key(2)] public string BranchName { get; set; } = string.Empty;
    [Key(3)] public string CompanyName { get; set; } = string.Empty;

    [Key(4)] public string Address { get; set; } = string.Empty;

    [Key(5)] public string? PhoneNumber { get; set; }

    [Key(6)] public bool IsActive { get; set; }
}